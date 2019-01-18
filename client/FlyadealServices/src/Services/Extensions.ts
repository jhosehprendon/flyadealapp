declare global {
    interface Array<T> {
        mapMany<U>(this: T[], callbackfn: (value: T, index: number, array: ReadonlyArray<T>) => U[], thisArg?: any): U[];
    }
}

export module Extensions {    
    if (!Array.prototype.mapMany) {
        Array.prototype.mapMany = function <T, U>(callback: (value: T, index: number, array: ReadonlyArray<T>) => U[]): U[] {
            const subRes: U[][] = this.map((item: T, index: number) => callback(item, index, this));
            if (!subRes.length) {
                return [];
            }
            return subRes.reduce((prev, cur) => prev.concat(cur));
        }
    }
}