/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   User.h
 * Author: Herman
 *
 * Created on May 5, 2023, 10:48 PM
 */

#ifndef USER_H
#define USER_H
#pragma once
#include <string>
#include <vector>
using namespace std;

#include "App.h"
#include "Attempt.h"

class User {
public:
    
    User();
    User(string username, string password);
    
    virtual void DisplayInfo();
    bool ViewQuizzesSummary(bool p, vector<Quiz*> quizzes);
    virtual void ReviewAttempt(Attempt* a) = 0;
    string GetUsername();
    string GetPassword();
    
    User(const User& orig);
    virtual ~User();
private:
    string username;
    string password;
};

#endif /* USER_H */

