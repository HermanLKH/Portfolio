/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   Answer.h
 * Author: Herman
 *
 * Created on May 5, 2023, 9:21 PM
 */

#ifndef ANSWER_H
#define ANSWER_H
#include <string>
#include <iostream>

using namespace std;

class Answer {
public:
    enum AnswerChoice{
        A=1, B=2, C=3, D=4
    };
    
    static AnswerChoice intToAnswerChoice(int i);
    static string AnswerChoiceToString(AnswerChoice ac);
    
    Answer();
    Answer(AnswerChoice ansChoice, string ansTxt);
    Answer(const Answer& orig);
    virtual ~Answer();
    
    void Display();
    AnswerChoice GetAnswerChoice();
private:
    AnswerChoice answerChoice;
    string answerText;
};

#endif /* ANSWER_H */

