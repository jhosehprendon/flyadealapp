import { AjaxRequestInterceptor, AjaxResponseInterceptor, SessionStorage, AjaxRequestInterceptorCallback, AjaxResponseInterceptorCallback } from '../Contracts/Contracts';
import SessionTokenStorage from './SessionTokenStorage';
import { AxiosRequestConfig, AxiosResponse } from 'axios';

export default class SessionTokenHeaderInterceptor implements AjaxRequestInterceptor, AjaxResponseInterceptor {
    private _header: string;
    private _sessionStorage: SessionTokenStorage;

    constructor(storage: SessionTokenStorage, header: string) {
        this._header = header;
        this._sessionStorage = storage;
    }
    
    getRequestCallback() : AjaxRequestInterceptorCallback {
        return (config: AxiosRequestConfig) => {
            const token = this._sessionStorage.sessionToken;
            if (token) {
                config.headers[this._header] = token;
            }
            return config;
        };
    }

    getResponseCallback() : AjaxResponseInterceptorCallback {
        return (response: AxiosResponse): Promise<AxiosResponse> | AxiosResponse => {
            const token = response.headers[this._header];
            if (token) {
                this._sessionStorage.sessionToken = token;
            }
            return response;
        };
    }
}