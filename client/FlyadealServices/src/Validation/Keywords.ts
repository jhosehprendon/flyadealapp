import moment from 'moment';
import { Keyword } from "./SchemaValidator";

const fdMomentKeyword: Keyword = {
    name: 'fd-moment',
    definition: {
        type: 'object',
        schema: true,
        // $data: true,
        compile: (schema: any, parentSchema: any) => {
            return (data, dataPath, parentData, propertyName) => {
                let result = moment(data).isValid();
                if (result && schema.minimum) {
                    result = moment(schema.minimum).isValid() && schema.minimum.isBefore(data);
                }
                if (result && schema.maximum) {
                    result = moment(schema.maximum).isValid() && schema.maximum.isAfter(data);
                }
                return result;
            }
        },
        //validate: function (data, dataPath, parentData, propertyName) {
        //    // some error handling/validation is needed here that the replacing object doesn't contains $refs e.g.
        //    var ref = data;
        //    var dataRef = jsonPointer.get(this.data, ref);
        //    parentData[propertyName] = dataRef; // that would replace the value of $ref with reference
        //    // if you want to replace { $ref: '...' } with reference you may use jsonPointer to do it.
        //}
    }
};

export default [ fdMomentKeyword ];