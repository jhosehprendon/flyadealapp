import React, { Component } from 'react';
import { View, TouchableHighlight, Text, StyleSheet } from 'react-native';
// import moment from 'moment';

// Assets
import commonStyles from '../../../../assets/styles/commonStyles';
import colors from '../../../../containers/colors';

export default class StationPicker extends Component {

    shouldComponentUpdate(newProps, newState) {
        return this.props.culture !== newProps.culture || 
                this.props.station !== newProps.station || 
                this.props.title !== newProps.title;
    }

    getStationName = () => {
        return this.props.station ? this.props.getStationName(this.props.station) : '';
    }

    onClick = () => {
        return this.props.onClick && this.props.onClick();
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
                            <Text style={[commonStyles.largeText, textStyle, textAlign, styles.stationName, direction]}>{this.getStationName()}</Text>
                            <Text style={[commonStyles.largeText, textStyle, textAlign, styles.stationCode, direction]}>{this.props.station || ''}</Text>
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
        justifyContent: 'center',
        paddingTop: 10,
        paddingBottom: 2,
        paddingHorizontal: 20
    },
    back: {
        flex: 1,
        backgroundColor: 'white',
        flexDirection: 'column',
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
    stationCode: {
        // fontSize: 18 * .8,
        // paddingLeft: 5,
        paddingRight: 5,
        color: 'black',
    },
    stationName: {
        paddingRight: 5,
        fontWeight: 'bold',
        // fontSize: // 18 * .8,
        color: 'black'
    }
});