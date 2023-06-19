/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   App.cpp
 * Author: Herman
 * 
 * Created on May 6, 2023, 12:26 AM
 */

#include "App.h"

App::App() {
    cout << "Welcome to SwinQuiz App" << endl;
    isActive = true;
}

string App::GetString(string prompt){
   string input;
    
    do{
        cout << prompt << endl;
        getline(cin, input);
//        cin.ignore(10000, '\n');
    }while(input.empty());
    
    return input; 
}

int App::GetInt(string prompt){
   int input;
    
    do{
        cout << prompt << endl;
        cin >> input;
        cin.ignore(10000, '\n');
    }while(cin.fail());
    
    return input; 
}

void App::DisplayMenu(){
    cout << "*********   MCQ Console Application   *********" << endl;
    switch(userMode){
        case Teacher:{
            cout << "1. Question Pool" << endl;
            cout << "2. Quiz" << endl;
            cout << "3. View profile info" << endl;
            cout << "4. Logout" << endl;
            
            opt = GetInt("Please select an option: ");
            
            switch(opt){
                case 1:{
                    cout << "1. Create question pool" << endl;
                    cout << "2. Remove Question Pool" << endl;
                    cout << "3. Add question to question pool" << endl;
                    cout << "4. Remove question from question pool" << endl;
                    cout << "5. View question pool" << endl;
                    cout << "6. Back" << endl;
                    
                    subOpt = GetInt("Please select an option: ");
                    break;
                }
                case 2:{
                    cout << "1. Create quiz" << endl;
                    cout << "2. Remove quiz" << endl;
                    cout << "3. Add question to quiz" << endl;
                    cout << "4. Remove question from quiz" << endl;
                    cout << "5. Publish quiz" << endl;
                    cout << "6. Review quiz" << endl;
                    cout << "7. View students attempts" << endl;
                    cout << "8. Back" << endl;
                    
                    subOpt = GetInt("Please select an option: ");
                    break;
                }
            }
            break;
        }
        case Student:{
            cout << "1. Attempt Quiz" << endl;
            cout << "2. Review attempt" << endl;
            cout << "3. View profile info" << endl;
            cout << "4. Logout" << endl;
            
            opt = GetInt("Please select an option: ");
            break;
        }
    }
}

App::UserMode App::GetUserMode(){
    return userMode;
}

bool App::GetIsActive(){
    return isActive;
}

void App::SetIsActive(bool isAct){
    isActive = isAct;
}

int App::GetOpt(){
    return opt;
}

int App::GetSubOpt(){
    return subOpt;
}

void App::SetUserMode(UserMode mode){
    userMode = mode;
}

App::App(const App& orig) {
}

App::~App() {
}

