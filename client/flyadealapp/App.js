import React from 'react';
import { StyleSheet, Text, View } from 'react-native';
import FlightSearchNative from './src/containers/modules/FlightSearch/FlightSearchNative';

export default class App extends React.Component {
  render() {
    return (
      <View style={styles.container}>
        <FlightSearchNative />
      </View>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',
  },
});
