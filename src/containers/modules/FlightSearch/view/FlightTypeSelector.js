import React, { Component } from 'react';
import { View, TouchableHighlight, Text, StyleSheet } from 'react-native';
import moment from 'moment';

// Assets
import commonStyles, { heightCoeff } from '../../../../assets/styles/commonStyles';
import colors from '../../../../containers/colors';

export default class FlightTypeSelector extends Component {

    shouldComponentUpdate(newProps) {
        return this.props.culture !== newProps.culture || 
                this.props.flightType !== newProps.flightType ||
                this.props.properties.returnTitle !== newProps.properties.returnTitle;
    }

    returnSelected = () => {
        return this.props.onChange('round');
    }

    oneWaySelected = () => {
        return this.props.onChange('oneway');
    }

    createRadio({ selected, text, onClick }) {
        const textStyle = this.props.culture === 'ar-SA' ? commonStyles.textArabic : commonStyles.text;
        const direction = this.props.culture === 'ar-SA' ? commonStyles.directionArabic : commonStyles.direction;
        return (<TouchableHighlight style={styles.button} underlayColor={colors.brandLight} onPress={onClick}>
                    <View style={[styles.button, direction]}>
                        <View style={styles.radioBack}>
                            {selected && <View style={styles.selected} />}
                        </View>
                        <Text style={selected ? [commonStyles.normalText, styles.text, textStyle, styles.active] : [commonStyles.normalText, styles.text, textStyle]}>{text}</Text>
                    </View>
                </TouchableHighlight>);
    }

    render() {
        const direction = this.props.culture === 'ar-SA' ? commonStyles.directionArabic : commonStyles.direction;
        return (
            <View style={[styles.container, direction, this.props.style]}>
                {this.createRadio({ selected: this.props.flightType === 'round', text: this.props.properties.returnTitle, onClick: this.returnSelected })}
                {this.createRadio({ selected: this.props.flightType === 'oneway', text: this.props.properties.oneWayTitle, onClick: this.oneWaySelected })}
            </View>
        );
    }
}

const styles = StyleSheet.create({
    button: {
        flex:1,
        justifyContent: "center",
        alignItems: "center",
        flexDirection: 'row',
        height: 23
    },
    active: {
        fontWeight: 'bold',
    },
    text: {
        paddingLeft: 10, 
        paddingRight: 10,
        textAlignVertical: 'center',
        textAlign: 'center',
        color: 'black'// colors.brandDark
    },
    container: {
        alignItems: 'center',
        flexDirection: 'row', 
        justifyContent: 'center',
    },
    radioBack: {
        // flex: 1,
        height: 23 * heightCoeff,
        width: 23 * heightCoeff,
        backgroundColor: 'white',
        alignItems: 'center',
        flexDirection: 'row', 
        // flex: 1, 
        justifyContent: 'center',
    },
    selected: {
        width: 15 * heightCoeff,
        height: 15 * heightCoeff,
        backgroundColor: colors.brandDark
    }
});