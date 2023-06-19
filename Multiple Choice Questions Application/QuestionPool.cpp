/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   QuestionPool.cpp
 * Author: Herman
 * 
 * Created on May 5, 2023, 9:49 PM
 */

#include "QuestionPool.h"


QuestionPool::QuestionPool(string pName) {
    poolName  = pName;
    numOfQuestions = 0;
}

QuestionPool::QuestionPool() {
}

string QuestionPool::Summary(){
    return poolName + ", " + to_string(numOfQuestions) + " questions" + "\n";
}

void QuestionPool::AddQuestion(Question* q){
    questions.push_back(q);
    numOfQuestions += 1;
}

void QuestionPool::RemoveQuestion(int i){
    if (i >= 0 && i < questions.size()) {
        questions.erase(questions.begin() + i);
        numOfQuestions -= 1;
    }
    else{
        cout << "Invalid index" << endl;
    }
}

void QuestionPool::DisplayQuestionsSummary(){
    int index = 1;
    
    for(Question* q : questions){
        cout << index << ". " << q->GetQuestionText() << endl;
        index += 1;
    }
}

Question* QuestionPool::GetQuestion(int i){
    return questions.at(i);
}

string QuestionPool::GetPoolName(){
    return poolName;
}

vector<Question*> QuestionPool::GetQuestions(){
    return questions;
}

QuestionPool::QuestionPool(const QuestionPool& orig) {
}

QuestionPool::~QuestionPool() {
}

