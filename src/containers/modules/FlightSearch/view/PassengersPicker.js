import React, { Component } from 'react';
import { View, Modal, Picker, TouchableHighlight, Text, StyleSheet, Platform } from 'react-native';
import moment from 'moment';

// Assets
import commonStyles from '../../../../assets/styles/commonStyles';
import colors from '../../../../containers/colors';

export default class PassengersPicker extends Component {

    shouldComponentUpdate(newProps, newState) {
        return this.props.title !== newProps.title ||
                this.props.culture !== newProps.culture || 
                this.props.adults !== newProps.adults || 
                this.props.children !== newProps.children || 
                this.props.infants !== newProps.infants;
    }

    onClick = () => this.props.onClick && this.props.onClick();

    createMessage = () => {
        const adultsLabel = this.props.adults < 2 ? this.props.properties.passengerTypeAdult : this.props.properties.passengerTypeAdultPlural;
        const childLabel = this.props.children < 2 ? this.props.properties.passengerTypeChild : this.props.properties.passengerTypeChildPlural;
        const infantLabel = this.props.infants < 2 ? this.props.properties.passengerTypeInfant : this.props.properties.passengerTypeInfantPlural;
        
        return <Text>
            <Text style={styles.paxNumber}>{this.props.adults} </Text>
            <Text>{adultsLabel}{this.props.children ? ', ': ' '}</Text>
            {this.props.children ? <Text style={styles.paxNumber}>{this.props.children} </Text> : ''}
            {this.props.children ? <Text>{childLabel}{this.props.infants ? ', ': ' '}</Text> : ''}
            {this.props.infants ? <Text style={styles.paxNumber}>{this.props.infants} </Text> : ''}
            {this.props.infants ? <Text>{infantLabel} </Text> : ''}
        </Text>;
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
        flexDirection: 'column'
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
    },
    titleSmallArabic: {
        flex: 4,
        paddingTop: 4,
        paddingRight: 10,
    },
    messageContainer: {
        flex: 7,
        paddingLeft: 10,
        paddingRight: 10
    },
    message: {
        paddingRight: 5,
        color: 'black',
    },
    paxNumber: {
        fontWeight: 'bold'
    }
});
