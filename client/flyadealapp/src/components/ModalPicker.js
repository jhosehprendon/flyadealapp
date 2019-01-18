import React, { Component } from 'react';
import {
    View,
    StyleSheet,
    Text,
    FlatList,
    TouchableOpacity
} from 'react-native';

import ModalContainer from './ModalContainer';

import colors from '../containers/colors';
import commonStyles, { fontAdjust, heightCoeff, height } from '../assets/styles/commonStyles';


export default class ModalPicker extends Component {

    shouldComponentUpdate(newProps, newState) {
        return this.props.rtl !== newProps.rtl || 
                this.props.selected !== newProps.selected || 
                this.props.cancelText !== newProps.cancelText;
    }

    onChange(item) {
        if(this.props.onChange) {
            this.props.onChange(item);
        }
        this.close();
    }

    close = () => {
        this.refs.modalContainer.close();
    }

    open = () => {
        this.refs.modalContainer.open();
    }

    renderSection(section) {
        const sectionStyle = [styles.sectionStyle, this.props.sectionStyle];
        const sectionTextStyle = this.props.rtl ? [commonStyles.textBoldArabic, styles.sectionTextStyle, this.props.sectionTextStyle, commonStyles.largeText] : 
            [commonStyles.textBold, styles.sectionTextStyle, this.props.sectionTextStyle, commonStyles.largeText];
        return (
            <View key={section.key} style={sectionStyle}>
                <Text style={sectionTextStyle}>{section.label}</Text>
            </View>
        );
    }

    renderOption(option) {
        
    }

    renderItem = (item) => {
        const option = item.item;
        const optionStyle = [styles.optionStyle, this.props.optionStyle];
        const optionTextStyle = this.props.rtl ? [commonStyles.textBoldArabic, styles.optionTextStyle, this.props.optionTextStyle, commonStyles.largeText] : 
            [commonStyles.textBold, styles.optionTextStyle, this.props.optionTextStyle, commonStyles.largeText];
        // return <Text>{option.label}</Text>;
        // console.log(option.key);
        return (<TouchableOpacity key={option.key} onPress={() => this.onChange(option)}>
                <View style={optionStyle}>
                    <Text style={optionTextStyle}>{option.label}</Text>
                </View>
            </TouchableOpacity>);
    }

    keyExtractor = (item) => item.key;

    // renderOptionList = () => {
    //     return (
    //         <View style={{paddingHorizontal:10}}>
    //             <FlatList 
    //                 data={this.props.data}
    //                 style={{flexGrow: 1}}
    //                 renderItem={this.renderItem}
    //                 keyExtractor={this.keyExtractor}
    //                 showsVerticalScrollIndicator={true}
    //              />
    //         </View>);
    // }

    renderContent = () => {
        const { rtl, data, title } = this.props;
        const textStyle = rtl ? commonStyles.textArabic : commonStyles.text;
        // const direction = rtl ? commonStyles.directionArabic : commonStyles.direction;
        return (
            <View style={styles.container}>
                <View style={styles.headerView}>
                    <Text style={[textStyle, styles.headerText, { fontWeight: 'bold' }, commonStyles.largeText, fontAdjust(rtl)]}>{title}</Text>
                </View>
                <View style={styles.contentView}>
                    <FlatList 
                        data={data}
                        style={{marginBottom: 20 * heightCoeff, flexGrow: 1}}
                        renderItem={this.renderItem}
                        keyExtractor={this.keyExtractor} />
                </View>
            </View>
        );
    }

    render() {
        return (<ModalContainer {...this.props}
            containerStyle={[styles.modalContainer, this.props.containerStyle]}
            ref='modalContainer' 
            getContent={this.renderContent} 
            okClicked={this.okClicked}
            onClose={this.onClose}
            // rtl={rtl}
        />);
    }
}

const styles = StyleSheet.create({
    modalContainer: {
        flex: .9,
        justifyContent: 'flex-start'
    },
    container: {
        flex: 1,
        // height: 50,
        // backgroundColor: 'pink',
        // flexGrow: 1,
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
        flex: 0,
        marginVertical: 15
        // backgroundColor: 'green'
    },
    headerText: {
        alignItems: 'center', 
        color: colors.brandDark, 
        textAlignVertical: 'center'
    },
    textInputContainer: {
        marginTop: 10 * heightCoeff,
        padding: 5 * heightCoeff,
        borderWidth: 1 * heightCoeff,
        borderColor: colors.greyBorder
    },

    contentView: {
        paddingHorizontal: 20,
        flex: 1
    },

    optionStyle: {
        padding: 14,
        borderBottomWidth: 1,
        borderBottomColor: '#ccc'
    },

    optionTextStyle: {
        textAlign: 'center',
        fontSize: 16,
        color: 'black'
        // color: colors.brandDark,
    },
});

// // const {height, width} = Dimensions.get('window');

// const PADDING = 14;
// const FONT_SIZE = 18;
// const HIGHLIGHT_COLOR = 'black';//'rgba(0,118,255,0.9)';
// // const optionContainerHeight = height * 0.65;

// const styles = StyleSheet.create({

//     optionStyle: {
//         padding: PADDING,
//         borderBottomWidth: 1,
//         borderBottomColor: '#ccc'
//     },

//     optionTextStyle: {
//         textAlign: 'center',
//         fontSize: FONT_SIZE,
//         color: HIGHLIGHT_COLOR
//         // color: colors.brandDark,
//     },

//     sectionStyle: {
//         padding: PADDING * 2,
//         borderBottomWidth: 1,
//         borderBottomColor: '#ccc'
//     },

//     sectionTextStyle: {
//         textAlign: 'center',
//         fontSize: FONT_SIZE,
//         color: colors.brandDark,
//     }
// });