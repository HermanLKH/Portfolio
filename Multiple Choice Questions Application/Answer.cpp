/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   Answer.cpp
 * Author: Herman
 * 
 * Created on May 5, 2023, 9:21 PM
 */

#include "Answer.h"

Answer::AnswerChoice Answer::intToAnswerChoice(int i) {
    switch (i) {
        case 1:
            return Answer::A;
        case 2:
            return Answer::B;
        case 3:
            return Answer::C;
        case 4:
            return Answer::D;
        default:
            break;
    }
}

string Answer::AnswerChoiceToString(AnswerChoice ac){
    switch(ac){
        case AnswerChoice::A:
            return "A";
        case AnswerChoice::B:
            return "B";
        case AnswerChoice::C:
            return "C";
        case AnswerChoice::D:
            return "D";
    }
}

Answer::Answer() {  
}

void Answer:: Display(){
    string ansChoice;
    
    switch(answerChoice){
        case AnswerChoice::A:
            ansChoice = "A";
            break;
        case AnswerChoice::B:
            ansChoice = "B";
            break;
        case AnswerChoice::C:
            ansChoice = "C";
            break;
        case AnswerChoice::D:
            ansChoice = "D";
            break;
    }
    cout << ansChoice << ". " << answerText << endl;
}

Answer::Answer(AnswerChoice ansChoice, string ansTxt) {
    answerChoice = ansChoice;
    answerText = ansTxt;
}

Answer::AnswerChoice Answer::GetAnswerChoice(){
    return answerChoice;
}

Answer::Answer(const Answer& orig) {
}

Answer::~Answer() {
}

