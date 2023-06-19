/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   Attempt.cpp
 * Author: Herman
 * 
 * Created on May 5, 2023, 10:34 PM
 */

#include "Attempt.h"

Attempt::Attempt() {
}

Attempt::Attempt(Quiz* q, int an) {
    quiz = q;
    attemptNo = an;
}

void Attempt::DisplayReview(){
    int index = 1;
    string status;
    
    cout << quiz->GetQuizTitle() << "(Attempt" << attemptNo << ")\t";
    cout << "[" << GetScore() << "/" << GetTotalScore() << "]" << endl;
    
    for(Question* q : quiz->GetQuestions()){
        if(scores[index - 1] == 1){
            status = "[Correct]";
        }
        else{
            status = "[Wrong]";
        }
        
        cout << index << ". " << q->GetQuestionText() << "\t" << status << endl;
        q->DisplayReview();
        
        index += 1;
    }
}

void Attempt::DisplayReviewSummary(){
    cout << quiz->GetQuizTitle() << "(Attempt" << attemptNo << ")" << ",\t";
    cout << "Score: [" << GetScore() << "/" << GetTotalScore() << "]" << endl;
}

int Attempt::GetTotalScore() {
    return quiz->GetQuestions().size();
}

int Attempt::GetScore(){
    int score = 0;
    
    for(int s : scores){
        score += s;
    }
    return score;
}

void Attempt::SetScores(){
    scores = quiz->Mark();
}

Quiz* Attempt::GetQuiz() {
    return quiz;
}

Attempt::Attempt(const Attempt& orig) {
}

Attempt::~Attempt() {
}

