/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   User.cpp
 * Author: Herman
 * 
 * Created on May 5, 2023, 10:48 PM
 */

#include "User.h"

User::User() {
    username = "";
    password = "";
}

User::User(string u, string p) {
    username = u;
    password = p;
}

void User::DisplayInfo(){
    cout << "Username: " << username << endl;
    cout << "Password: " << password << endl;
}

bool User::ViewQuizzesSummary(bool p, vector<Quiz*> quizzes){
    int index = 1;
    
    for(Quiz* q : quizzes){
        if(q->GetIsPublished() == p){
            cout << index << ". " << q->Summary();
            index += 1; 
        }
    }
    if(index == 1){
        cout << "No quiz" << endl;
        return false;
    }
    return true;
}

string User::GetUsername(){
    return username;
}

string User::GetPassword(){
    return password;
}

User::User(const User& orig) {
}

User::~User() {
}

