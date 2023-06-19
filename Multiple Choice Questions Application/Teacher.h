/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   Teacher.h
 * Author: Herman
 *
 * Created on May 5, 2023, 11:48 PM
 */

#ifndef TEACHER_H
#define TEACHER_H
#pragma once
#include <string>
#include <vector>
#include <iostream>

using namespace std;

#include "Attempt.h"
#include "User.h"

class Teacher: public User {
public:
    Teacher();
    Teacher(string sNo, string u, string p);
    
    void DisplayInfo();
    
    void CreateQuestionPool(vector<QuestionPool*> &questionPools);
    void RemoveQuestionPool(int i, vector<QuestionPool*> &questionPools);
    void ReviewQuestionPool(QuestionPool* qp, vector<QuestionPool*> questionPools);
    
    void ViewSummary(vector<QuestionPool*> questionPools);
    
    void AddQuestion(QuestionPool* qp);
    void RemoveQuestion(QuestionPool* qp, int i);
    void AddQuestion(Quiz* qz, int i);
    void RemoveQuestion(Quiz* qz, int i);
    
    void CreateQuiz(vector<QuestionPool*> questionPools, vector<Quiz*> &quizzes);
    void RemoveQuiz(int i, vector<Quiz*> &quizzes);
    void ReviewQuiz(Quiz* qz);
    void PublishQuiz(Quiz* qz);
    
    void ReviewAttempt(Attempt* a);
    
    Teacher(const Teacher& orig);
    virtual ~Teacher();
private:
    string staffNo;
    
};

#endif /* TEACHER_H */

