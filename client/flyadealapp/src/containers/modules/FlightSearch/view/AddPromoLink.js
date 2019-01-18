import React, { Component } from 'react';
import { View, TouchableHighlight, Text, StyleSheet, Dimensions } from 'react-native';

// Assets
import commonStyles from '../../../../assets/styles/commonStyles';
import colors from '../../../../containers/colors';

// import homeIcon from '../../../../../assets/images/home_icon.png';

export default class AddPromoLink extends Component {

    shouldComponentUpdate(newProps) {
        return this.props.culture !== newProps.culture ||
            this.props.message !== newProps.message;
    }

    onPress = () => {
        if(this.props.onPress && typeof(this.props.onPress) === 'function') {
            this.props.onPress();
        }
    }

    render() {
        const textStyle = this.props.culture === 'ar-SA' ? commonStyles.textBoldArabic : commonStyles.textBold;
        const direction = this.props.culture === 'ar-SA' ? commonStyles.directionArabic : commonStyles.direction;
        const hightLightStyle = [this.props.style, direction, styles.highlight];
        return (
            <TouchableHighlight onPress={this.onPress}
                underlayColor={colors.brandLight}
                style={hightLightStyle}>
                <Text style={[commonStyles.normalText, textStyle, styles.buttonText]}>{this.props.message}</Text>
            </TouchableHighlight>
        );
    }
}

const styles = StyleSheet.create({
    highlight: {
        alignItems: 'center', 
        justifyContent: 'center'
    },
    buttonText: {
        textAlign: 'center', 
        textAlignVertical: 'center', 
        textDecorationLine: 'underline', 
        color: colors.brandDark
    }
});