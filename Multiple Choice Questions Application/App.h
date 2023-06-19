/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   App.h
 * Author: Herman
 *
 * Created on May 6, 2023, 12:26 AM
 */

#ifndef APP_H
#define APP_H
#include <cstdlib>
#include <string>
#include <vector>
#include <iostream>

using namespace std;

class App {
public:
    enum UserMode{
        Teacher, Student
    };

    static string GetString(string prompt);
    static int GetInt(string prompt);
    App();
    
    void DisplayMenu();
    UserMode GetUserMode();
    bool GetIsActive();
    void SetIsActive(bool isAct);
    void SetUserMode(UserMode mode);
    int GetOpt();
    int GetSubOpt();
    
    App(const App& orig);
    virtual ~App();
private:
    bool isActive;
    UserMode userMode;
    int opt;
    int subOpt;
};

#endif /* APP_H */

