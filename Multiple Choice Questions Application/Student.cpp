/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   Student.cpp
 * Author: Herman
 * 
 * Created on May 6, 2023, 2:34 PM
 */

#include "Student.h"

Student::Student() {
}

Student::Student(string sID, string u, string p):User(u, p){
    studID = sID;
}

void Student::AttemptQuiz(Quiz* q){
    int count = 0;
    
    for(Attempt* a : attempts){
        if(a->GetQuiz() == q){
            count += 1;
        }
    }
    bool isValid = count < q->GetAttemptLimit();
    
    if(isValid){
        q->Display();
    
        Attempt* a = new Attempt(q, count + 1);
        a->SetScores();
        attempts.push_back(a);
    }
    else{
        cout << "No attempt left" << endl;
    }
}

void Student::ReviewAttempt(Attempt* a){
    a->DisplayReview();
}

void Student::DisplayInfo(){
    cout << "Student ID: " << studID << endl;
    User::DisplayInfo();
}

vector<Attempt*> Student::GetAttemptsByQuiz(Quiz* q){
    vector<Attempt*> attemptsBQ;
    int index = 1;
    
    for(Attempt* a : attempts){
        if(a->GetQuiz() == q){
            cout << "Attempt " << index << endl;
            attemptsBQ.push_back(a);
            index += 1;
        }
    }
    return attemptsBQ;
}

vector<Attempt*> Student::GetAttempts(){
    return attempts;
}

string Student::GetStudId() {
    return studID;
}


Student::Student(const Student& orig) {
}

Student::~Student() {
}

