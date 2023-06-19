/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   Quiz.cpp
 * Author: Herman
 * 
 * Created on May 5, 2023, 10:22 PM
 */

#include "Quiz.h"


//remove quiz by question pool
//void Quiz::RemoveQuiz(QuestionPool* qp){
//    for(Quiz* qz : quizzes){
//        if(qz->questionPool == qp){
//            auto newEnd = remove(quizzes.begin(), quizzes.end(), qz);
//            quizzes.erase(newEnd, quizzes.end());
//        }
//    }
//}

Quiz::Quiz() {
}

Quiz::Quiz(string qTitle, QuestionPool* qp, int al){
    quizTitle = qTitle;
    questionPool = qp;
    attemptLimit = al;
    isPublished = false;
    numOfQuestions = 0;
}

string Quiz::Summary(){
    return quizTitle + ", " + to_string(numOfQuestions) + " questions \n";
}

void Quiz::DisplayReview(){
    cout << "Quiz title: " << quizTitle << endl;
    cout << "Number of questions: " << numOfQuestions << endl;
    cout << "Question pool: " << questionPool->GetPoolName() << endl;
    cout << "Attempt limit: " << attemptLimit << endl;
    
    if(isPublished){
        cout << "Status: Published" << endl;
    }
    else{
        cout << "Status: Not published" << endl;
    }
}

void Quiz::Display(){
    string input;
    int index = 1;
    
    cout << quizTitle << endl;
    
    for(Question* q : questions){
        cout << index << ". ";
        q->Display();
        
        do{
            input = App::GetString("Please select an answer: ");
        }while(input.empty() || input.length() != 1);
    
        q->SetAnswerChosen(input);
        
        index += 1;
    }
}

void Quiz::DisplayQuestionsSummary(){
    int index = 1;
    
    for(Question* q : questions){
        cout << index << ". " << q->GetQuestionText() << endl;
        index += 1;
    }
}

vector<int> Quiz::Mark(){
    vector<int> scores;
    
    for (Question* question : questions) {
        scores.push_back(question->Mark());
    }
    
    return scores;
}

void Quiz::AddQuestion(int i){
    questions.push_back(questionPool->GetQuestion(i));
    numOfQuestions += 1;
}

void Quiz::RemoveQuestion(int i) {
    if(i < questions.size()){
        questions.erase(questions.begin() + i);
        numOfQuestions -= 1;
    }
    else{
        cout << "Invalid index" << endl;
    }
}

QuestionPool* Quiz::GetQuestionPool(){
    return questionPool;
}

string Quiz::GetQuizTitle(){
    return quizTitle;
}

vector<Question*> Quiz::GetQuestions(){
    return questions;
}

int Quiz::GetAttemptLimit(){
    return attemptLimit;
}

void Quiz::SetIsPublished(bool isP){
    isPublished = isP;
}

bool Quiz::GetIsPublished(){
    return isPublished;
}

Quiz::Quiz(const Quiz& orig) {
}

Quiz::~Quiz() {
}

