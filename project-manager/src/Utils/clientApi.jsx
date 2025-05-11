import axios from 'axios';
import { TOKEN_GHN } from '../Contains/Config';
import { BASE_URL } from '../Contains/ConfigURL';


export const callApi = (endpoint, method = "get", data = null) => {
    return axios({
        method,
        url: `${BASE_URL}/${endpoint}`,
        data,
        headers: {
            "Content-Type" : "application/json",
            "Authorization" : "Bearer " + TOKEN_GHN,
        }
    });
}



export const callApiMuti = (endpoint, method = "get", data = null) => {
    return axios({
        method,
        url: `${BASE_URL}/${endpoint}`,
        data,
        headers: {
            'accept': 'text/plain',
            "Content-Type" : "multipart/form-data",
            "Authorization" : "Bearer " + TOKEN_GHN,
        }
    });
    
}
