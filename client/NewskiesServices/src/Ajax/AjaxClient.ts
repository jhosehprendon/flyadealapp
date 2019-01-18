import { AjaxClientConfig, AjaxRequestInterceptor, AjaxResponseInterceptor } from '../Contracts/Contracts';
import Axios, { AxiosInstance, AxiosResponse, AxiosRequestConfig } from "axios";
import Utils from "../Services/Utils";

export default class AjaxClient {

    private readonly _instance: AxiosInstance;
    private readonly _config: AjaxClientConfig;

    constructor(config: AjaxClientConfig) {
        this._config = config;
        this._instance = config.baseUrl ? Axios.create({ baseURL: config.baseUrl }) : Axios.create();
        if (config.requestInterceptors && config.requestInterceptors.length) {
            this._config.requestInterceptors.forEach(i => {
                this.registerRequestInterceptor(i);
            });
        }
        if (config.responseInterceptors && config.responseInterceptors.length) {
            this._config.responseInterceptors.forEach(i => this.registerResponseInterceptor(i));
        }
    }

    async get (url: string): Promise<AxiosResponse> {
        try {
            return await this._instance.get(url);
        } catch (e) {
            throw Utils.getNewskiesError(e);
        }
    }

    async post(url: string, data?: any): Promise<AxiosResponse> {
        try {
            return await this._instance.post(url, data);
        } catch (e) {
            throw Utils.getNewskiesError(e);
        }
    }

    async put(url: string, data?: any): Promise<AxiosResponse> {
        try {
            return await this._instance.put(url, data);
        } catch (e) {
            throw Utils.getNewskiesError(e);
        }
    }

    async delete(url: string): Promise<AxiosResponse> {
        try {
            return await this._instance.delete(url);
        } catch (e) {
            throw Utils.getNewskiesError(e);
        }
    }

    async registerRequestInterceptor(interceptor: AjaxRequestInterceptor) {
        interceptor.requestId = this._instance.interceptors.request.use(interceptor.getRequestCallback(), interceptor.getRequestErrorCallback ? interceptor.getRequestErrorCallback() : undefined);
    }

    async registerResponseInterceptor(interceptor: AjaxResponseInterceptor) {
        interceptor.responseId = this._instance.interceptors.response.use(interceptor.getResponseCallback(), interceptor.getResponseErrorCallback ? interceptor.getResponseErrorCallback() : undefined);
    }
}