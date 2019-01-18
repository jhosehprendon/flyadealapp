import { SessionStorage } from '../Contracts/Contracts';

export default class SessionTokenStorage {

    private _sessionStorage: SessionStorage;

    constructor(storage: SessionStorage) {
        this._sessionStorage = storage;
    }

    get sessionToken(): string {
        return this._sessionStorage.getItem('token');
    }

    set sessionToken(value: string) {
        this._sessionStorage.setItem('token', value);
    }
}