import { StyleSheet, Dimensions, Platform } from 'react-native';

import colors from '../../containers/colors';
import deviceInfo from 'react-native-device-info';

const model = deviceInfo.getModel();
const { height, width } = Dimensions.get('window');

const isIphoneX = model === 'iPhone X';
// const IOS6_HEIGHT = 667; // using it as a benchmark
const IOS6_WIDTH = 375;// using it as a benchmark
const heightCoeff = 1 - (IOS6_WIDTH - width) * 0.003652727;
const IOS_TOP_SCREEN_PADDING = isIphoneX ? 30 : 20;
const layoutTopPadding = (Platform.OS === 'ios') ? IOS_TOP_SCREEN_PADDING : 0;
const layoutHeight = (Platform.OS === 'ios') ? height : height - IOS_TOP_SCREEN_PADDING;

const fontAdjust = (rtl, size=5) => rtl ? {marginVertical: -size, backgroundColor: 'transparent'} : {};

export { heightCoeff, height, width, layoutTopPadding, fontAdjust, layoutHeight};

export default StyleSheet.create({

    mainLayout: {
        paddingTop: layoutTopPadding,
        // paddingBottom: 20,
        height: layoutHeight,
        backgroundColor: isIphoneX ? colors.brandDark : 'white'
    },

    text: {
        fontFamily: (Platform.OS === 'ios') ? 'Raleway' : 'raleway',
        color: 'black'
    },

    textBold: {
        fontFamily: (Platform.OS === 'ios') ? 'Raleway' : 'raleway',
        fontWeight: 'bold',
        color: 'black'
    },

    textArabic: {
        fontFamily: (Platform.OS === 'ios') ? 'Cairo' : 'cairo',
        marginVertical: 0,
        color: 'black'
    },

    textBoldArabic: {
        fontFamily: (Platform.OS === 'ios') ? 'Cairo' : 'cairo',
        fontWeight: 'bold',
        color: 'black'
    },

    headerMenu: {
        alignItems: 'center', 
        justifyContent: 'space-between', 
        flexDirection: 'row',
        backgroundColor: colors.brandDark, 
        // marginTop: 15 * heightCoeff,
        height: 60 * heightCoeff,
    },

    footerMenu: {
        alignItems: 'center', 
        justifyContent: 'space-between', 
        flexDirection: 'row',
        backgroundColor: colors.brandDark, 
        marginTop: 15 * heightCoeff,
        height: 60 * heightCoeff,
    },

    footerButton: {
        justifyContent: 'center',
        alignItems: 'center'
    },
    
    footerHomeButton: {
        paddingHorizontal: 8 * heightCoeff,
    },
    footerIcon: {
        height: 30 * heightCoeff
    },

    footerMenuIcon: {
        height: 25 * heightCoeff
    },

    footerUserButton: {
        paddingHorizontal: 20 * heightCoeff
    },

    footerUserBackground: {
        justifyContent: 'center',
        alignItems: 'center',
        width: 40 * heightCoeff,
        borderRadius:20 * heightCoeff,
        height: 40 * heightCoeff,
        backgroundColor: 'white'
    },

    directionArabic: {
        flexDirection: 'row-reverse'
    },

    direction: {
        flexDirection: 'row'
    },

    // flight search font sizes
    xxsmallText: {
        fontSize : 11 * heightCoeff,
        // lineHeight: 16 * heightCoeff,
    },

    xsmallText: {
        fontSize : 12 * heightCoeff,
        // lineHeight: 18 * heightCoeff,
    },

    smallText: {
        fontSize : 13 * heightCoeff,
        // lineHeight: 20 * heightCoeff,
    },

    normalText: {
        fontSize : 15 * heightCoeff,
        // lineHeight: 23 * heightCoeff,
    },

    largeText: {
        fontSize : 18 * heightCoeff,
        // lineHeight: 21.5 * heightCoeff,
        // backgroundColor: 'pink',
        // includeFontPadding: false
    },

    xlargeText: {
        fontSize : 21 * heightCoeff,
        // lineHeight: 28 * heightCoeff,
    },

    xxlargeText: {
        fontSize : 24 * heightCoeff,
        // lineHeight: 32 * heightCoeff,
    },


    blackTitle: {
        fontSize : 14,
        fontWeight : 'bold'
    },

    panel : {
        padding : 10
    },

    parentPanel : {
        flex : 1,
        padding : 0
    },

    halfPanel : {
        flex : 2,
        padding : 7
    },

    thirdPanel : {
        flex : 1
    },

    firstPanel : {
        borderRightWidth : 1,
        borderColor : colors.inputBorder
    },

    bigText : {
        fontSize : 14,
        fontWeight : 'bold'
    },

    payment: {
        margin: 10,
        borderWidth : 1,
        borderColor : colors.inputBorder,
    },

    lightBorder : {
        borderWidth : 1,
        borderColor : colors.inputBorder,
    },

    borderRadius : {
        borderRadius : 3,
    },

    paymentText : {
        minHeight : 20,
        paddingHorizontal : 5,
        paddingBottom : 1,
        paddingTop : 5
    },

    borderRight: {
        borderRightWidth: 1,
        borderColor: colors.dividerBorder
    },

    borderBottom: {
        borderBottomWidth: 1,
        borderColor: colors.dividerBorder
    },

    titleIcon : {
        fontSize : 18
    },

    innerElem : {
        paddingRight : 10
    },

    opposite : {
        flex : 2,
        justifyContent: 'space-between',
        alignItems : 'center'
    },

    singleTextInput : {
        borderWidth : 1,
        borderColor : colors.inputBorder,
        padding : 5,
        minHeight : 37,
        borderRadius : 3,
        backgroundColor : colors.white,
        marginTop : 15
    },

    singleLocationInput : {
        marginTop : 5

    },

    leftIcon : {
        marginRight : 10
    },

    blackContainer : {
        backgroundColor: colors.blackBackground,
        marginHorizontal : 0,
        marginVertical : 0,
        padding : 10
    },

    blackContainerText : {
        color: colors.white
    }
});