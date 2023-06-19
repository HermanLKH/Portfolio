/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   Student.h
 * Author: Herman
 *
 * Created on May 6, 2023, 2:34 PM
 */

#ifndef STUDENT_H
#define STUDENT_H
#pragma once
#include <string>
#include <vector>
#include <iostream>

using namespace std;

#include "User.h"
#include "Attempt.h"

class Student: public User {
public:
    Student();
    Student(string sID, string u, string p);
    
    void AttemptQuiz(Quiz* q);
    void ReviewAttempt(Attempt* a);
    
    void DisplayInfo();
    vector<Attempt*> GetAttemptsByQuiz(Quiz* q);
    vector<Attempt*> GetAttempts();
    string GetStudId();
    
    Student(const Student& orig);
    virtual ~Student();
private:
    string studID;
    vector<Attempt*> attempts;
};

#endif /* STUDENT_H */

