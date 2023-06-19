/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   Quiz.h
 * Author: Herman
 *
 * Created on May 5, 2023, 10:22 PM
 */

#ifndef QUIZ_H
#define QUIZ_H
#pragma once
#include <string>
#include <vector>
#include <iostream>
#include <algorithm>

using namespace std;

#include "App.h"
#include "QuestionPool.h"

class Quiz {
public:
    Quiz();
    Quiz(string qTitle, QuestionPool* qp, int al);
    
    string Summary();
    void DisplayReview();
    void Display();
    void DisplayQuestionsSummary();
    vector<int> Mark();
    void AddQuestion(int i);
    void RemoveQuestion(int i);
    
    QuestionPool* GetQuestionPool();
    string GetQuizTitle();
    int GetAttemptLimit();
    void SetIsPublished(bool isP);
    bool GetIsPublished();
    vector<Question*> GetQuestions();
    
    Quiz(const Quiz& orig);
    virtual ~Quiz();
private:
    string quizTitle;
    int attemptLimit;
    int numOfQuestions;
    bool isPublished;
    vector<Question*> questions;
    QuestionPool* questionPool;
};

#endif /* QUIZ_H */

