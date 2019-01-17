import React, { Component } from 'react';
import { View, Modal, Picker, TouchableHighlight, Text, StyleSheet, Platform } from 'react-native';
import moment from 'moment';

// Assets
import commonStyles from '../../../../assets/styles/commonStyles';
import colors from '../../../../containers/colors';

export default class FlightDatesPicker extends Component {

    shouldComponentUpdate(newProps, newState) {
        return this.props.culture !== newProps.culture || 
                this.props.startDate.diff(newProps.startDate) !== 0 || 
                this.props.endDate.diff(newProps.endDate) !== 0 ||
                this.props.flightType !== newProps.flightType;
    }

    onClick = () => {
        return this.props.onClick && this.props.onClick();
    }

    createMessage = () => {
        const startDate = moment(this.props.startDate).locale(this.props.culture.toLowerCase());
        const endDate = moment(this.props.endDate).locale(this.props.culture.toLowerCase());
        if(this.props.culture === 'ar-SA') {
            return this.props.flightType !== 'oneway' ? 
                `${startDate.date()} ${startDate.format('MMMM')} ${startDate.year()} - ${endDate.date()} ${endDate.format('MMMM')} ${endDate.year()}` : 
                `${startDate.date()} ${startDate.format('MMMM')} ${startDate.year()}`;
        }
        return this.props.flightType !== 'oneway' ? 
            `${startDate.format('ddd,DD MMM YYYY')} - ${endDate.format('ddd,DD MMM YYYY')}` : 
            `${startDate.format('ddd,DD MMM YYYY')}`;
    }

    render() {
        const direction = this.props.culture === 'ar-SA' ? commonStyles.directionArabic : commonStyles.direction;
        const textAlign = this.props.culture === 'ar-SA' ? styles.alignArabic : styles.align;
        const textStyle = this.props.culture === 'ar-SA' ? commonStyles.textArabic : commonStyles.text;
        const titleSmall = this.props.culture === 'ar-SA' ? styles.titleSmallArabic : styles.titleSmall;
        return (
                <TouchableHighlight style={[styles.container, direction, this.props.style]} underlayColor={colors.brandLight} onPress={this.onClick}>
                    <View style={styles.back}>
                        <Text style={[commonStyles.smallText, textStyle, textAlign, titleSmall, direction]}>{this.props.title}</Text>
                        <View style={[styles.messageContainer, direction]}>
                            <Text style={[commonStyles.largeText, textStyle, textAlign, styles.message, direction]}>{this.createMessage()}</Text>
                        </View>
                    </View>
                </TouchableHighlight>
        );
    }
}

const styles = StyleSheet.create({
    container: {
        alignItems: 'center',
        flexDirection: 'row', 
        // flex: 1, 
        justifyContent: 'center',
        paddingTop: 10,
        paddingBottom: 2,
        paddingLeft: 20,
        paddingRight: 20,
        // backgroundColor: 'green'
    },
    back: {
        flex: 1,
        backgroundColor: 'white',
        flexDirection: 'column',
        //height: 62
    },
    alignArabic: {
        textAlign: 'right',
    },
    align: {
        textAlign: 'left',
    },
    titleSmall: {
        flex: 4,
        // fontSize: 13 * .8,
        paddingTop: 9,
        paddingLeft: 10,
        // backgroundColor: 'yellow',
    },
    titleSmallArabic: {
        flex: 4,
        // fontSize: 13 * .8,
        paddingTop: 4,
        paddingRight: 10,
        // backgroundColor: 'yellow',
    },
    messageContainer: {
        flex: 7,
        paddingLeft: 10,
        paddingRight: 10,
        //backgroundColor: 'pink'
    },
    message: {
        // fontSize: 18 * .8,
        paddingRight: 5,
        color: 'black',
    }
});