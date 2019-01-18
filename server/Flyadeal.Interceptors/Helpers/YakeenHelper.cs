using Flyadeal.Interceptors.Yakeen4Flyadeal;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Flyadeal.Interceptors.Helpers
{
    public static class YakeenHelper
    {
        internal static short GetYakeenCountryCode(string twoLetterCountryCode)
        {
            if (CountryMap.ContainsKey(twoLetterCountryCode))
            {
                return CountryMap[twoLetterCountryCode];
            }
            return 1;
        }

        private static readonly Dictionary<string, short> CountryMap = new Dictionary<string, short>
        {
            {"AD", 522 },
            {"AE", 101 },
            {"AF", 301 },
            {"AI", 639 },
            {"AL", 502 },
            {"AM", 539 },
            {"AN", 660 },
            {"AO", 438 },
            {"AR", 601 },
            {"AS", 809 },
            {"AT", 523 },
            {"AU", 701 },
            {"AZ", 343 },
            {"BA", 545 },
            {"BB", 603 },
            {"BD", 305 },
            {"BE", 509 },
            {"BF", 424 },
            {"BG", 508 },
            {"BH", 103 },
            {"BI", 404 },
            {"BJ", 413 },
            {"BM", 636 },
            {"BO", 626 },
            {"BQ", 647 },
            {"BR", 604 },
            {"BS", 612 },
            {"BW", 403 },
            //{"BW", 445 },
            {"BY", 537 },
            {"CA", 610 },
            {"CC", 661 },
            //{"CC", 709 },
            {"CD", 426 },
            {"CH", 515 },
            {"CI", 418 },
            {"CK", 657 },
            {"CL", 627 },
            {"CM", 425 },
            {"CN", 325 },
            //{"CN", 314 },
            {"CO", 611 },
            {"CR", 613 },
            {"CU", 614 },
            {"CX", 708 },
            {"CY", 326 },
            {"CZ", 552 },
            {"DE", 503 },
            {"DJ", 203 },
            {"DK", 512 },
            {"DM", 615 },
            {"DO", 616 },
            {"DZ", 202 },
            {"EC", 628 },
            {"EE", 543 },
            {"EG", 207 },
            {"ER", 449 },
            {"ES", 501 },
            {"ET", 401 },
            {"FI", 517 },
            {"FJ", 801 },
            {"FK", 654 },
            {"FO", 554 },
            {"FR", 516 },
            {"GB", 506 },
            //{"GE", 664 },
            {"GE", 541 },
            {"GF", 655 },
            {"GH", 421 },
            {"GI", 534 },
            {"GL", 634 },
            {"GM", 409 },
            {"GN", 422 },
            {"GR", 521 },
            {"GT", 619 },
            {"GU", 810 },
            {"GW", 423 },
            {"GQ", 440 },
            {"HK", 322 },
            {"HR", 546 },
            {"HT", 620 },
            {"HU", 525 },
            {"ID", 302 },
            {"IE", 504 },
            {"IN", 321 },
            {"IQ", 105 },
            {"IR", 303 },
            {"IS", 526 },
            {"IT", 505 },
            {"JM", 607 },
            {"JO", 102 },
            {"JP", 323 },
            {"KE", 427 },
            {"KG", 340 },
            {"KH", 317 },
            {"KM", 410 },
            //{"KN", 638 },
            {"KN", 663 },
            {"KP", 328 },
            {"KR", 318 },
            {"KW", 109 },
            {"KY", 642 },
            {"KZ", 336 },
            {"LA", 329 },
            {"LB", 110 },
            {"LC", 624 },
            {"LK", 313 },
            {"LR", 429 },
            {"LS", 428 },
            {"LT", 542 },
            {"LU", 528 },
            {"LV", 544 },
            {"LY", 206 },
            {"MA", 208 },
            {"MC", 530 },
            {"ME", 524 },
            {"MF", 651 },
            {"MG", 818 },
            {"MH", 813 },
            {"MK", 549 },
            {"ML", 430 },
            {"MM", 307 },
            //{"MM", 352 },
            //{"MM", 353 },
            //{"MM", 351 },
            {"MN", 330 },
            {"MO", 331 },
            {"MP", 811 },
            {"MR", 209 },
            {"MS", 643 },
            {"MT", 529 },
            {"MU", 433 },
            {"MV", 310 },
            {"MX", 622 },
            {"MY", 319 },
            {"MZ", 434 },
            {"NA", 412 },
            {"NC", 817 },
            {"NE", 436 },
            {"NF", 706 },
            {"NG", 435 },
            {"NL", 519 },
            {"NO", 531 },
            {"NP", 320 },
            {"NZ", 702 },
            {"OM", 106 },
            {"PA", 605 },
            {"PE", 630 },
            {"PF", 815 },
            {"PG", 703 },
            {"PH", 315 },
            {"PK", 304 },
            {"PL", 510 },
            {"PM", 633 },
            {"PR", 652 },
            {"PS", 107 },
            //{"PS", 125 },
            //{"PS", 121 },
            //{"PS", 123 },
            //{"PS", 124 },
            //{"PS", 122 },
            {"PT", 507 },
            {"PY", 629 },
            {"QA", 108 },
            {"RO", 513 },
            {"RS", 518 },
            {"RU", 311 },
            {"RW", 414 },
            {"SB", 804 },
            {"SC", 443 },
            {"SD", 204 },
            {"SE", 514 },
            {"SG", 312 },
            {"SH", 451 },
            {"SI", 547 },
            {"SK", 553 },
            {"SM", 532 },
            {"SN", 419 },
            {"SO", 205 },
            {"SR", 631 },
            {"SS", 453 },
            {"SV", 617 },
            {"SY", 104 },
            {"TD", 405 },
            {"TG", 407 },
            {"TH", 308 },
            {"TJ", 339 },
            {"TL", 349 },
            {"TM", 338 },
            {"TN", 201 },
            {"TO", 805 },
            {"TR", 309 },
            {"TV", 806 },
            {"TZ", 406 },
            {"UA", 536 },
            {"UG", 402 },
            {"US", 601 },
            {"UY", 632 },
            {"UZ", 337 },
            {"VC", 625 },
            {"VE", 609 },
            {"VG", 641 },
            {"VI", 653 },
            {"VN", 316 },
            {"WF", 816 },
            {"WS", 808 },
            {"YE", 111 },
            {"YT", 452 },
            {"ZA", 411 },
            {"ZM", 417 },
            {"ZW", 415 },
        };
    }

    public sealed class YakeenClient : Yakeen4FlydealClient
    {
        private static volatile YakeenClient instance;
        private static object syncRoot = new Object();

        private YakeenClient() { }

        public static YakeenClient Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new YakeenClient();
                            //instance.InnerChannel.OperationTimeout = Constants.YakeenTimeout;
                            //instance.Endpoint.Address = new EndpointAddress(Constants.YakeenServiceUrl);
                        }
                    }
                }

                return instance;
            }
        }
    }
}
