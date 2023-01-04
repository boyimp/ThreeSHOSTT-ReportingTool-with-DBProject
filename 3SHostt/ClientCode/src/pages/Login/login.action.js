//In the name of Allah

import axios from 'axios';
import GloblaConstant from '../../services/_utils/global.data';

export default async function isVarifiedUser(user) {
    const url = GloblaConstant.BASE_URL + GloblaConstant.ROUTES_IS_VERIFIED_USER;
    const data = user;
    return axios
        .post(url, data)
        .then((response) => {
            if (response.status === 200) {
                var result = response.data;
                var finalResult =
                    result.Username === data.Username && result.Passsword === data.Passsword
                        ? true
                        : false;
                return finalResult;
            } //if
        })
        .catch((error) => {
            return error;
        });
} //func

export async function logout() {
    const url = GloblaConstant.BASE_URL + GloblaConstant.ROUTES_LOG_OUT;
    return axios
        .get(url)
        .then((response) => {
            if (response.status === 200) {
                return response.data;
            } //if
        })
        .catch((error) => {
            return error;
        });
} //func

export async function isSessionActive() {
    const url = GloblaConstant.BASE_URL + GloblaConstant.ROUTES_IS_SESSION_ACTIVE;
    return axios
        .get(url)
        .then((response) => {
            if (response.status === 200) {
                return response.data;
            } //if
        })
        .catch((error) => {
            return error;
        });
} //func
