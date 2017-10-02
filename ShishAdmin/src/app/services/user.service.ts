import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';

import { AngularFireAuth } from 'angularfire2/auth';
import * as firebase from 'firebase/app';

//import { Api } from './api';
import { AuthService } from './auth.service';

@Injectable()
export class UserService {
    isLoggedIn = false;

    constructor(public afAuth: AngularFireAuth, public authService: AuthService) {
        
    }

    login(providerString) {
        var self = this;
        var promise = new Promise<any>((resolve, reject) => {
            var provider = null;
            switch (providerString) {
                case 'google': {
                    provider = new firebase.auth.GoogleAuthProvider();
                    break;
                }
                case 'facebook': {
                    provider = new firebase.auth.FacebookAuthProvider();
                    break;
                }
                default: {
                    throw 'ERR: PROVIDER NOT SELECTED';
                }
            }

            self.afAuth.app.auth().signInWithPopup(provider).then(function (result) {
                if (result.user) {
                    result.user.getIdToken().then(function (accessToken) {
                        //todo set this as a model
                        var userData = {
                            accessToken: accessToken,
                            displayName: result.user.displayName,
                            photoUrl: result.user.photoURL,
                            email: result.user.email,
                            providerData: result.user.providerData
                        };

                        self.authService.getToken(userData).then((resp) => {
                            resolve(resp);
                        }, (err) => {
                            reject(err);
                        });
                    });
                } else {
                    resolve(null);
                }
            }).catch(function (error) {
                //TODO handle exception
                //For example when multiple accounts with same email
                reject(error);
            });
        });

        return promise;
    }

    signWithUserAndPass(email, password) {
        var self = this;

        var promise = new Promise<any>((resolve, reject) => {
            self.afAuth.app.auth().signInWithEmailAndPassword(email, password).then(function (user) {
                if (user) {
                    user.getIdToken().then(function (accessToken) {
                        //todo set this as a model
                        var userData = {
                            accessToken: accessToken,
                            displayName: user.displayName,
                            photoUrl: user.photoURL,
                            email: user.email,
                            providerData: user.providerData
                        };

                        self.authService.getToken(userData).then((resp) => {
                            resolve(resp);
                        }, (err) => {
                            reject(err);
                        });
                    });
                } else {
                    resolve(null);
                }
            }).catch(function (error: firebase.FirebaseError) {
                reject(error.message);
            });
        });

        return promise;
    }

    createFirebaseUser(email, password, username) {
        var self = this;
        var promise = new Promise<any>((resolve, reject) => {
            self.afAuth.app.auth().createUserWithEmailAndPassword(email, password).then(function (user) {
                if (user) {
                    user.getIdToken().then(function (accessToken) {
                        //todo set this as a model
                        var userData = {
                            accessToken: accessToken,
                            displayName: username,
                            photoUrl: user.photoURL,
                            email: user.email,
                            providerData: user.providerData
                        };

                        self.authService.getToken(userData).then((resp) => {
                            resolve(resp);
                        }, (err) => {
                            reject(err);
                        });
                    });
                } else {
                    resolve(null);
                }
            }).catch(function (error: firebase.FirebaseError) {
                reject(error.message);
            });
        });

        return promise;
    }
}