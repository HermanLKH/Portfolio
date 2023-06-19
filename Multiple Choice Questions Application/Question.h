/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   Question.h
 * Author: Herman
 *
 * Created on May 5, 2023, 9:17 PM
 */

#ifndef QUESTION_H
#define QUESTION_H
#pragma once
#include <string>
#include <vector>
#include <iostream>

#include "Answer.h"
#include "App.h"

using namespace std;

class Question {
public:
    Question();
    Question(string qText, vector<Answer*> as, int c);
    
    void Display();
    void DisplayReview();
    int Mark();
    void AddAnswer(int i);
    
    string GetQuestionText();
    void SetAnswerChosen(string input);
    
    Question(const Question& orig);
    virtual ~Question();
private:
    string questionText;
    vector<Answer*> answers;
    Answer* correctAnswer;
    Answer* selectedAnswer;
};

#endif /* QUESTION_H */

