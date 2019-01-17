import React, { Component } from 'react';
import { View, TouchableHighlight, Text, StyleSheet } from 'react-native';

// Assets
import commonStyles from '../assets/styles/commonStyles';
import colors from '../containers/colors';

export default class Button extends Component {

    shouldComponentUpdate(newProps) {
        return this.props.rtl !== newProps.rtl ||
            this.props.message !== newProps.message;
    }

    onPress = () => {
        if(this.props.onPress && typeof(this.props.onPress) === 'function') {
            this.props.onPress();
        }
    }

    render() {
        const { rtl, style, message } = this.props;
        const textStyle = rtl ? commonStyles.textBoldArabic : commonStyles.textBold;
        const direction = rtl ? commonStyles.directionArabic : commonStyles.direction;
        return (
            <View style={[style, direction]}>
                <TouchableHighlight onPress={this.onPress}
                    underlayColor={colors.brandDark}
                    style={styles.highlight}>
                    <Text style={[commonStyles.largeText, textStyle, styles.buttonText]}>{message}</Text>
                </TouchableHighlight>
            </View>
        );
    }
}

const styles = StyleSheet.create({
    highlight: {
        flex:1, 
        alignItems: 'center', 
        justifyContent: 'center', 
        marginHorizontal: 20, 
        backgroundColor: colors.brandDark
    },
    buttonText: {
        textAlign: 'center', 
        textAlignVertical: 'center', 
        color: 'white'
    }
});
