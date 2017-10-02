import { Injectable } from '@angular/core';

@Injectable()
export class StorageService {
    setValue(key: string, value: any) {
        localStorage.setItem(key, value);
    }

    setObject(key: string, value: any) {
        localStorage.setItem(key, JSON.stringify(value));
    }

    getValue(key: string) {
        return localStorage.getItem(key);
    }

    getObject(key: string) {
        return JSON.parse(localStorage.getItem(key));
    }

    removeValue(key: string) {
        localStorage.removeItem(key);
    }
}
