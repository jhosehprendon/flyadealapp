using Microsoft.Extensions.Options;
using Newskies.WebApi.Configuration;
using dto = Newskies.WebApi.Contracts;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Navitaire.WebServices.DataContracts.Booking;
using Navitaire.WebServices.DataContracts.Common.Enumerations;
using System.Linq;
using Newskies.WebApi.Contracts;

namespace Newskies.WebApi.Services
{
    public interface IFeeService
    {
        Task<dto.SellResponse> SellFee(dto.SellFeeRequestData sellFeeRequestData);
        Task<dto.CancelResponse> CancelFee(dto.CancelFeeRequestData cancelFeeRequestData);
    }

    public class FeeService : ServiceBase, IFeeService
    {
        private readonly IBookingManager _client;
        private readonly ISessionBagService _sessionBagService;
        private readonly IResourcesService _resourceService;
        private readonly IBookingService _bookingService;

        public FeeService(IBookingManager client, ISessionBagService sessionBagService, IBookingService bookingService, IResourcesService resourceService, IOptions<AppSettings> appSettings) : base(appSettings)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _sessionBagService = sessionBagService ?? throw new ArgumentNullException(nameof(sessionBagService));
            _resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
            _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
        }

        public async Task<dto.SellResponse> SellFee(dto.SellFeeRequestData sellFeeRequestData)
        {
            var feeList = await _resourceService.GetFeeList(await _sessionBagService.CultureCode());
            var fee = feeList.FeeList.ToList().Find(f => f.FeeCode == sellFeeRequestData.FeeCode);
            if (fee == null)
            {
                throw new ResponseErrorException(dto.Enumerations.ResponseErrorCode.InvalidFee, new string[] { "Fee not found. " });
            }
            var booking = await _sessionBagService.Booking();
            sellFeeRequestData.SellFeeType = fee.FeeType == dto.Enumerations.FeeType.ServiceFee ? dto.Enumerations.SellFeeType.ServiceFee
                : fee.FeeType == dto.Enumerations.FeeType.PenaltyFee ? dto.Enumerations.SellFeeType.PenaltyFee 
                : dto.Enumerations.SellFeeType.Unmapped;
            sellFeeRequestData.CollectedCurrencyCode = booking.CurrencyCode;
            sellFeeRequestData.PassengerNumber = sellFeeRequestData.PassengerNumber;

            var response = await _client.SellAsync(new SellRequest
            {
                ContractVersion = _navApiContractVer,
                EnableExceptionStackTrace = false,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBagService.Signature(),

                SellRequestData = new SellRequestData
                {
                    SellBy = SellBy.Fee,
                    SellFee = new SellFee
                    {
                        SellFeeRequestData = Mapper.Map<Navitaire.WebServices.DataContracts.Booking.SellFeeRequestData>(sellFeeRequestData)
                    }
                }
            });
            return Mapper.Map<dto.SellResponse>(response);
        }

        public async Task<dto.CancelResponse> CancelFee(dto.CancelFeeRequestData cancelFeeRequestData)
        {
            var booking = await _bookingService.GetSessionBooking();
            var pax = booking.Passengers.ToList().Find(p => p.PassengerNumber == cancelFeeRequestData.PassengerNumber);
            if (pax == null)
            {
                throw new ResponseErrorException(dto.Enumerations.ResponseErrorCode.InvalidPassengerNumber, "Invalid passenger number. " );
            }
            var fee = pax.PassengerFees.ToList().Find(f => f.FeeCode == cancelFeeRequestData.FeeCode);
            if (fee == null)
            {
                throw new ResponseErrorException(dto.Enumerations.ResponseErrorCode.FeeNotFound, "Fee not found. ");
            }
            var response = await _client.CancelAsync(new CancelRequest
            {
                ContractVersion = _navApiContractVer,
                EnableExceptionStackTrace = false,
                MessageContractVersion = _navMsgContractVer,
                Signature = await _sessionBagService.Signature(),
                CancelRequestData = new CancelRequestData
                {
                    CancelBy = CancelBy.Fee,
                    CancelFee = new CancelFee
                    {
                        FeeRequest = new FeeRequest
                        {
                            PassengerNumber = cancelFeeRequestData.PassengerNumber,
                            //NetAmount = 
                            CurrencyCode = booking.CurrencyCode,
                            FeeNumber = fee.FeeNumber
                        }
                    }

                }
            });
            return Mapper.Map<dto.CancelResponse>(response);
        }
    }
}