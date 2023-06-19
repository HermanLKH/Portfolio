/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   UserManagement.cpp
 * Author: Herman
 * 
 * Created on May 6, 2023, 3:41 PM
 */

#include "UserManagement.h"

UserManagement::UserManagement() {
}

User* UserManagement::ValidateLogin(App &app){
    int max = 3;
    bool isLogin = false;
    string username;
    string password;
    
    while(!isLogin && max != 0){
        username = App::GetString("Enter your username: ");
        password = App::GetString("Enter your password: ");
        
        for(User* t: teachers){
            if(username == t->GetUsername() && password == t->GetPassword()){
                isLogin = true;
                app.SetUserMode(App::Teacher);
                return t;
            }
        }
        for(User* s : students){
            if(username == s->GetUsername() && password == s->GetPassword()){
                isLogin = true;
                app.SetUserMode(App::Student);
                return s;
            }
        }
        max -= 1;
        cout << "Invalid username or password. Please try again." << endl;
    }
}

void UserManagement::registerTeacher(){
    string staffID = App::GetString("Enter your staff id: ");
    string username = App::GetString("Enter your username: ");
    string password = App::GetString("Enter your password: ");
    
    Teacher* newT = new Teacher(staffID, username, password);
    teachers.push_back(newT);
}

void UserManagement::registerStudent(){
    string studID = App::GetString("Enter your student id: ");
    string username = App::GetString("Enter your username: ");
    string password = App::GetString("Enter your password: ");
    
    Student* newS = new Student(studID, username, password);
    students.push_back(newS);
}

void UserManagement::RegisterUser(){
    
    string input =  App::GetString("1. Teacher\n2. Student\n");
    
    if(input == "1"){
        registerTeacher();
    }else if(input == "2"){
        registerStudent();
    }
}

vector<User*> UserManagement::GetStudents(){
    return students;
}

UserManagement::UserManagement(const UserManagement& orig) {
}

UserManagement::~UserManagement() {
}

