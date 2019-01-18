import React, { Component } from 'react';
import {
    View,
    StyleSheet,
    Text,
    // ScrollView,
    // TouchableOpacity,
    // Dimensions
} from 'react-native';
import moment from 'moment';
import Dates from './ReactNativeDates';

import ModalContainer from './ModalContainer';

import colors from '../containers/colors';
import commonStyles, { fontAdjust } from '../assets/styles/commonStyles';


export default class ModalDates extends Component {

    state = {
        date: null,
        focus: 'startDate',
        startDate: null,
        endDate: null
      };

    shouldComponentUpdate(newProps, newState) {
        /*return (this.state.startDate && !this.state.startDate.isSame(newState.startDate)) || 
                (this.state.endDate && !this.state.endDate.isSame(newState.endDate)) ||
                (this.state.date && !this.state.date.isSame(newState.date)) ||
                this.props.culture !== newProps.culture || 
                !this.props.startDate.isSame(newProps.startDate) || 
                !this.props.endDate.isSame(newProps.endDate) ||
                this.props.okText !== newProps.okText || 
                this.props.cancelText !== newProps.cancelText;*/
        return this.opened === true;
    }

    /* componentWillReceiveProps(nextProps, nextState) {
        if (nextProps.startDate.diff(this.state.startDate) !== 0) {
            // this.setState({ ...this.state, startDate })
        }
        if (nextProps.endDate.diff(this.state.endDate) !== 0) {
            // this.setState({ ...this.state, endDate })
        }
    }*/

    onChange(item) {
        this.props.onChange(item);
        this.close();
    }

    onClose = () => {
        this.opened = false;
    }

    /*close = () => {
        this.refs.modalContainer.close();
    }*/

    open = () => {
        // this.props.startDate.locale(this.props.culture);
        // this.props.endDate.locale(this.props.culture);
        this.setState({ ...this.state, startDate: this.props.startDate, endDate: this.props.endDate, date: this.props.startDate }, () => {
            this.refs.modalContainer.open();
            this.opened = true;
        });
    }

    isDateBlocked = (date) =>
      date.isBefore(moment(), 'day');

    onDatesChange = ({ startDate, endDate, focusedInput }) => {
        this.setState({ ...this.state, focus: focusedInput }, () =>
            this.setState({ ...this.state, startDate, endDate, date: startDate })
        );
    }

    onDateChange = ({ date }) => {
      this.setState({ ...this.state, date, startDate: date, endDate: date });
    }

    onDatesSelected = async () => {
        this.opened = false;
        if(!this.props.datesChanged) {
            return;
        }
        if(this.isOneWay()) {
            return await this.props.datesChanged(this.state.date, this.state.date);
        }
        return await this.props.datesChanged(this.state.startDate, this.state.endDate);
    }

    isOneWay = () => this.props.startDate.isSame(this.props.endDate);

    renderContent = () => {
        const rtl = this.props.culture === 'ar-SA';
        const textStyle = rtl ? commonStyles.textBoldArabic : commonStyles.textBold;
        // const direction = this.props.culture === 'ar-SA' ? commonStyles.directionArabic : commonStyles.direction;
        return (
            <View style={styles.container}>
                <View style={styles.headerView}>
                    <Text style={[styles.headerText, textStyle, commonStyles.largeText]}>Select travel dates</Text>
                </View>
                <Dates
                    onDatesChange={this.onDatesChange}
                    onDateChange={this.onDateChange}
                    isDateBlocked={this.isDateBlocked}
                    startDate={this.state.startDate}
                    date={this.state.date}
                    endDate={this.state.endDate}
                    focusedInput={this.state.focus}
                    previousLabel={'< Previous'}
                    nextLabel={'Next >'}
                    range={!this.isOneWay()}
                    culture={this.props.culture}
                    navigationTextStyle={[textStyle, commonStyles.normalText, styles.navigationTextStyle]}
                    navigationContainerStyle={[styles.navigationContainerStyle]}
                    dayNameStyle={[textStyle, commonStyles.normalText, styles.dayNameStyle, fontAdjust(rtl)]}
                    dayStyle={[textStyle, commonStyles.normalText, styles.dayNameStyle, fontAdjust(rtl)]}
                    rtl={rtl}
                />
            </View>
        );
    }

    render() {
        return (
            <ModalContainer {...this.props}
                containerStyle={styles.modalContainer}
                ref='modalContainer' 
                getContent={this.renderContent} 
                okClicked={this.onDatesSelected}
                onClose={this.onClose}
                rtl={this.props.culture === 'ar-SA'}
            />);
    }
}

const styles = StyleSheet.create({
    modalContainer: {
        flex: .8
    },
    container: {
        flex: 1,
        flexGrow: 1,
        // marginTop: 20
    },
    date: {
        marginTop: 50
    },
    focused: {
        color: 'blue'
    },
    headerView: {
        alignItems: 'center', 
        justifyContent: "center",
        flex:0,
        marginVertical: 15
        // backgroundColor: 'green'
    },
    headerText: {
        alignItems: 'center', 
        color: colors.brandDark, 
        textAlignVertical: 'center'
    },

    navigationTextStyle: {
        color: colors.brandDark
    },
    navigationContainerStyle: {

    },
    dayNameStyle: {
        color: colors.brandDark
    }
});