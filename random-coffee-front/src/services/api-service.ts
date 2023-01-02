import axios, { AxiosInstance } from 'axios';

//TODO Вот тут основная суть, сюда будут запросы к серверу, а прокси из package.json уберем
const DATA_URL = '';
const REQUEST_TIMEOUT = 5000;

export const createAPI = (): AxiosInstance => (
	axios.create({
		baseURL: DATA_URL,
		timeout: REQUEST_TIMEOUT,
	})
);
