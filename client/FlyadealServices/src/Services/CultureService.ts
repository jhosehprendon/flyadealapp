import { Contracts as NskContracts, Utils as NskUtils } from 'newskies-services';
import Culture = NskContracts.Culture;
import { LocalStorage } from "../Contracts";
import ResourceCache from "./ResourceCache";

export default class CultureService {
    private _storageKey: string = 'culture';
    private _localStorage: LocalStorage;
    private _resourceCache: ResourceCache;
    private _defaultCulture: string

    constructor(localStorage: LocalStorage, resourceCache: ResourceCache, defaultCulture?: string) {
        this._localStorage = localStorage;
        this._resourceCache = resourceCache;
        this._defaultCulture = defaultCulture;
    }

    async getCultures(): Promise<NskContracts.Culture[]> {
        return await this._resourceCache.getCultures();
    }

    async setCurrentCulture(code: string): Promise<void> {
        if (!code || !code.length) {
            throw NskUtils.createNewskiesError(NskContracts.ErrorType.BadData, new Error(`CultureService, culture code parameter"${code}" can not be empty`));
        }
        if (!await this.checkCulture(code)) {
            return;
        }
        this._localStorage.setItem(this._storageKey, code);
    }

    async getCurrentCulture(): Promise<string> {
        let currentCulture = this._localStorage.getItem(this._storageKey);
        if (!currentCulture && !this._defaultCulture) {
            throw NskUtils.createNewskiesError(NskContracts.ErrorType.ConfigurationError, new Error(`CultureService, no culture is set and default culture wasn't provided`));
        }
        if (!await this.checkCulture(currentCulture)) {
            this._localStorage.setItem(this._storageKey, this._defaultCulture);
            currentCulture = this._defaultCulture;
        }
        return currentCulture;
    }

    private async checkCulture(code: string): Promise<boolean> {
        const cultures = await this.getCultures();
        const culture = cultures ? cultures.find(c => c.code === code) : undefined;
        return culture !== undefined;
    }
}