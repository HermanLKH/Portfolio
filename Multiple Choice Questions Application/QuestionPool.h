/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   QuestionPool.h
 * Author: Herman
 *
 * Created on May 5, 2023, 9:49 PM
 */

#ifndef QUESTIONPOOL_H
#define QUESTIONPOOL_H
#pragma once
#include <string>
#include <vector>
#include <iostream>

#include "Question.h"

using namespace std;

class QuestionPool {
public:
    QuestionPool();
    QuestionPool(string pName);
    
    string Summary();
    void DisplayQuestionsSummary();
    void AddQuestion(Question* q);
    void RemoveQuestion(int i);
    Question* GetQuestion(int i);
    string GetPoolName();
    vector<Question*> GetQuestions();  //    for test purpose
    
    QuestionPool(const QuestionPool& orig);
    virtual ~QuestionPool();
private:
    string poolName;
    vector<Question*> questions;
    int numOfQuestions;
};

#endif /* QUESTIONPOOL_H */

