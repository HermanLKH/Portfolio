/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   Question.cpp
 * Author: Herman
 * 
 * Created on May 5, 2023, 9:17 PM
 */

#include "Question.h"

Question::Question() {
}

Question::Question(string qText, vector<Answer*> as, int c){
    questionText = qText;
    answers = as;
    correctAnswer = answers[c];
}

void Question::AddAnswer(int i){
    string aText = App::GetString("Please enter an answer: ");
    Answer::AnswerChoice ac = Answer::intToAnswerChoice(i);
    
    Answer* a = new Answer(ac, aText);
    answers.push_back(a);
}

void Question::Display() {
    cout << questionText << endl;
    for(Answer* a : answers){
        a->Display();
    }
}

void Question::DisplayReview() {
    cout << "Your answer: " << Answer::AnswerChoiceToString(selectedAnswer->GetAnswerChoice()) << endl;
    cout << "Correct answer: " << Answer::AnswerChoiceToString(correctAnswer->GetAnswerChoice()) << endl;
}

void Question::SetAnswerChosen(string input){
    Answer::AnswerChoice ansChoice;
   
//    convert user input to answer choice enum
    if(input == "A"){
        ansChoice = Answer::AnswerChoice::A;
    }
    else if(input == "B"){
        ansChoice = Answer::AnswerChoice::B;
    }
    else if(input == "C"){
        ansChoice = Answer::AnswerChoice::C;
    }
    else if(input == "D"){
        ansChoice = Answer::AnswerChoice::D;
    }
//    select the student's answer for this question
    for(Answer* a : answers){
        if(ansChoice == a->GetAnswerChoice()){
            selectedAnswer = a;
            break;
        }
    }
}

int Question::Mark(){
    if(selectedAnswer == correctAnswer){
        return 1;
    }
    else{
        return 0;
    }
}

string Question::GetQuestionText(){
    return questionText;
}

Question::Question(const Question& orig) {
}

Question::~Question() {
}

