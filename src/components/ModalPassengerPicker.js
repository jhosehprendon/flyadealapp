import React, { Component } from 'react';
import {
    View,
    StyleSheet,
    Text,
    ScrollView,
    TouchableHighlight,
    Dimensions
} from 'react-native';

import ModalContainer from './ModalContainer';

import colors from '../containers/colors';
import commonStyles, { heightCoeff } from '../assets/styles/commonStyles';

class PassengerSelector extends Component {
    
    shouldComponentUpdate(newProps) {
        return this.props.number !== newProps.number ||
                this.props.label !== newProps.label ||
                this.props.rtl !== newProps.rtl;
    }

    remove = () => this.props.remove ? this.props.remove() : undefined;

    add = () => this.props.add ? this.props.add() : undefined;

    render() {
        const textStyle = this.props.rtl ? commonStyles.textBoldArabic : commonStyles.textBold;
        const direction = this.props.rtl ? commonStyles.directionArabic : direction;
        return (
            <View style={[paxStyles.container, direction]}>
                <Text style={[textStyle, commonStyles.normalText, paxStyles.label]}>{this.props.label}</Text>
                <TouchableHighlight style={paxStyles.button} underlayColor={colors.brandDark} onPress={this.remove}>
                    <Text style={paxStyles.buttonText}>-</Text>
                </TouchableHighlight>
                <Text style={[paxStyles.numberText, textStyle, commonStyles.normalText]}>{this.props.number}</Text>
                <TouchableHighlight style={paxStyles.button} underlayColor={colors.brandDark} onPress={this.add}>
                    <Text style={paxStyles.buttonText}>+</Text>
                </TouchableHighlight>
            </View>);
    }
}

const paxStyles = StyleSheet.create({
    container: {
        alignItems: 'center',
        justifyContent: 'flex-end',
        flexDirection: 'row', 
        margin: 17,
        // backgroundColor: 'green',
        // flex: 1
    },
    label: {
        paddingHorizontal: 20,
        color: 'black'
    },
    button: {
        width: 40,
        height: 40,
        alignItems: 'center',
        justifyContent: 'center',
        backgroundColor: colors.brandDark
    },
    buttonText: {
        fontWeight: 'bold',
        fontSize: 32,
        color: 'white'
    },
    numberText: {
        width:40,
        textAlign: 'center',
        color: 'black'
    },
    headerContainer: {
    },
    headerText: {
        textAlign: 'center',
        fontSize: FONT_SIZE,
        color: colors.brandDark,
        padding: PADDING
    }
});


export default class ModalPassengerPicker extends Component {

    shouldComponentUpdate(newProps, newState) {
        return this.props.rtl !== newProps.rtl || 
                this.props.cancelText !== newProps.cancelText || 
                this.props.adults !== newProps.adults || 
                this.props.children !== newProps.children || 
                this.props.infants !== newProps.infants;
    }

    onChange(item) {
        this.props.onChange(item);
        this.close();
    }

    close = () => this.refs.modalContainer.close();
    open = () => this.refs.modalContainer.open();
    addAdult = () => this.props.addAdult();
    addChild = () => this.props.addChild();
    addInfant = () => this.props.addInfant();
    removeAdult = () => this.props.removeAdult();
    removeChild = () => this.props.removeChild();
    removeInfant = () => this.props.removeInfant();

    renderPassengerOptions = () => {
        const adultsLabel = this.props.adults < 2 ? this.props.properties.passengerTypeAdult : this.props.properties.passengerTypeAdultPlural;
        const childLabel = this.props.children < 2 ? this.props.properties.passengerTypeChild : this.props.properties.passengerTypeChildPlural;
        const infantLabel = this.props.infants < 2 ? this.props.properties.passengerTypeInfant : this.props.properties.passengerTypeInfantPlural;

        const sectionTextStyle = this.props.rtl ? [commonStyles.textBoldArabic, styles.sectionTextStyle, this.props.sectionTextStyle, commonStyles.largeText] : 
            [commonStyles.textBold, styles.sectionTextStyle, this.props.sectionTextStyle, commonStyles.largeText];
        return <View style={[styles.container]}>
            <View style={styles.headerContainer}>
                <Text style={[styles.headerText, sectionTextStyle]}>Please select passengers</Text>
            </View>
            <View style={styles.optionsContainer}>
                <View>
                    <PassengerSelector 
                        number={this.props.adults} 
                        label={adultsLabel} 
                        rtl={this.props.rtl} 
                        add={this.addAdult} 
                        remove={this.removeAdult}/>
                </View>
                <View>
                    <PassengerSelector 
                        number={this.props.children} 
                        label={childLabel} 
                        rtl={this.props.rtl} 
                        add={this.addChild}
                        remove={this.removeChild}/>
                </View>
                <View>
                    <PassengerSelector 
                        number={this.props.infants} 
                        label={infantLabel} 
                        rtl={this.props.rtl} 
                        add={this.addInfant}
                        remove={this.removeInfant}/>
                </View>
            </View>
            
        </View>;
    }

    render() {
        return (<ModalContainer {...this.props} 
                    containerStyle={styles.modalContainer}
                    ref='modalContainer' 
                    getContent={this.renderPassengerOptions} 
                />);
    }
}

const {height, width} = Dimensions.get('window');

const PADDING = 14;
const FONT_SIZE = 18;
const HIGHLIGHT_COLOR = 'black';//'rgba(0,118,255,0.9)';
const optionContainerHeight = height * 0.65;

const styles = StyleSheet.create({
    modalContainer: {
        flex: .65
    },
    container: {
        flex: 1
    },
    headerContainer: {
        alignItems: 'center',
        marginTop: 20 * heightCoeff,
        justifyContent: 'center',
    },
    headerText: {
        textAlign: 'center',
        fontSize: FONT_SIZE,
        color: colors.brandDark,
        padding: PADDING * heightCoeff
    },
    optionsContainer: {
        // alignItems: 'center',
        // flexDirection: 'row', 
        paddingHorizontal: width * .18,
        flex: 1, 
        justifyContent: 'center',
        // backgroundColor: 'yellow'
    },
});