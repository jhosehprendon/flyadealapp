import React, { Component } from 'react';
import { View } from 'react-native';
import moment from 'moment';
import FlightTypeSelector from './FlightTypeSelector';
import StationPicker from './StationPicker';
import FlightDatesPicker from './FlightDatesPicker';
import PassengersPicker from './PassengersPicker';
import AddPromoLink from './AddPromoLink';

import ModalPicker from '../../../../components/ModalPicker';
import ModalDates from '../../../../components/ModalDates';
import ModalPassengerPicker from '../../../../components/ModalPassengerPicker';
import Button from '../../../../components/Button';

// Assets
// import commonStyles from '../../../../../assets/styles/native/commonStyles';
// import colors from '../../../../../containers/colors';

// import homeIcon from '../../../../../assets/images/home_icon.png';

export default class FlightSearchView extends Component {

    shouldComponentUpdate(newProps) {
        return this.props.options.culture !== newProps.options.culture ||
            this.props.options.flightType !== newProps.options.flightType ||
            this.props.options.originCode !== newProps.options.originCode ||
            this.props.options.destinationCode !== newProps.options.destinationCode ||
            this.props.options.adults !== newProps.options.adults ||
            this.props.options.children !== newProps.options.children ||
            this.props.options.infants !== newProps.options.infants ||

            this.props.options.originLocationSelect.length !== newProps.options.originLocationSelect.length ||
            this.props.options.destinationLocationSelect.length !== newProps.options.destinationLocationSelect.length ||

            moment(this.props.options.departureDate).diff(newProps.options.departureDate) !== 0 ||
            moment(this.props.options.arriveDate).diff(newProps.options.arriveDate) !== 0;
    }

    flightTypeChange = (type) => this.props.actions.setFlightType(type);

    getStationName = (code) => this.props.actions.getStationName(code);

    openOriginPicker = () => this.refs.originPicker.open();

    originStationChanged = (item) => this.props.actions.selectOrigin(item.key);

    openDestinationPicker = () => this.refs.destinationPicker.open();

    destinationStationChanged = (item) => this.props.actions.selectDestination(item.key);

    openTravelDatesPicker = () => this.refs.datesPicker.open();

    travelDatesChanged = async (startDate, endDate) => await this.props.actions.setFlightDate(startDate, endDate);

    openPassengersModalPicker = () => this.refs.modalPassengersPicker.open();

    addAdult = () => this.props.actions.addAdult();

    addChild = () => this.props.actions.addChild();

    addInfant = () => this.props.actions.addInfant();

    removeAdult = () => this.props.actions.removeAdult();

    removeChild = () => this.props.actions.removeChild();

    removeInfant = () => this.props.actions.removeInfant();

    searchFlights = async () => await this.props.actions.searchFlights();

    render() {
        const { style, properties, options } = this.props;
        // const originPickerStations = [{section: true, key: 'originStation', label: 'Select Origin'}, 
        //     ...this.props.options.originLocationSelect.map(i=>{ return { key: i.value, label: i.name}})];
        // const destinationPickerStations = [{section: true, key: 'destinationStation', label: 'Select Destination'}, 
        //     ...this.props.options.destinationLocationSelect.map(i=>{ return { key: i.value, label: i.name}})];
        return (
            <View style={style}>
                <FlightTypeSelector 
                    style={{flex: .9}}
                    properties={properties}
                    flightType={options.flightType} 
                    culture={options.culture} 
                    onChange={this.flightTypeChange} />
                <StationPicker 
                    style={{flex: 1}}
                    properties={properties}
                    station={options.originCode}
                    title={properties.originTitle}
                    getStationName={this.getStationName}
                    onClick={this.openOriginPicker}
                    culture={options.culture} />
                <StationPicker 
                    style={{flex: 1}}
                    properties={properties}
                    station={options.destinationCode}
                    title={properties.destinationTitle}
                    getStationName={this.getStationName}
                    onClick={this.openDestinationPicker}
                    culture={options.culture} />
                <FlightDatesPicker 
                    style={{flex: 1}}
                    properties={properties}
                    startDate={options.departureDate}
                    endDate={options.arriveDate}
                    flightType={options.flightType} 
                    title={properties.datePickerTitle}
                    onClick={this.openTravelDatesPicker}
                    culture={options.culture} />
                <PassengersPicker 
                    style={{flex: 1}}
                    properties={properties}
                    adults={options.adults}
                    children={options.children}
                    infants={options.infants}
                    title={properties.passengerSelectTitle}
                    onClick={this.openPassengersModalPicker}
                    culture={options.culture} />

                <AddPromoLink style={{flex: .7}}
                    culture={options.culture}
                    message={properties.promoCodeTitle} />
                <Button style={{flex: .8}}
                    rtl={options.culture === 'ar-SA'}
                    message={properties.searchBtnLabel}
                    onPress={this.searchFlights} />
                

                <ModalPicker ref='originPicker'
                    data={options.originLocationSelect.map(i=>{ return { key: i.value, label: i.name}})}
                    title={'Select Origin'}
                    // selected={options.originCode}
                    rtl={options.culture === 'ar-SA'}
                    onChange={this.originStationChanged}
                    cancelText={properties.closeLabel} />
                <ModalPicker ref='destinationPicker'
                    data={options.destinationLocationSelect.map(i=>{ return { key: i.value, label: i.name}})}
                    title={'Select Destination'}
                    selected={options.destinationCode}
                    rtl={options.culture === 'ar-SA'}
                    onChange={this.destinationStationChanged}
                    cancelText={properties.closeLabel} />
                <ModalDates ref='datesPicker'
                    culture={options.culture}
                    datesChanged={this.travelDatesChanged}
                    startDate={options.departureDate}
                    endDate={options.arriveDate}
                    okText={properties.okLabel}
                    cancelText={properties.cancelLabel} />
                <ModalPassengerPicker ref='modalPassengersPicker' 
                    rtl={options.culture === 'ar-SA'}
                    properties={properties}
                    // culture={options.culture}
                    addAdult={this.addAdult}
                    addChild={this.addChild}
                    addInfant={this.addInfant}
                    removeAdult={this.removeAdult}
                    removeChild={this.removeChild}
                    removeInfant={this.removeInfant}
                    adults={options.adults}
                    children={options.children}
                    infants={options.infants}
                    cancelText={properties.closeLabel} />
            </View>
        );
    }
}