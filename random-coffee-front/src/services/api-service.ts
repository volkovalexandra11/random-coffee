import axios, { AxiosInstance } from 'axios';

const DATA_URL = '';
const REQUEST_TIMEOUT = 5000;

export const createAPI = (): AxiosInstance => (
	axios.create({
		baseURL: DATA_URL,
		timeout: REQUEST_TIMEOUT,
	})
);
