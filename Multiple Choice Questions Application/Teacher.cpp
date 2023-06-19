/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   Teacher.cpp
 * Author: Herman
 * 
 * Created on May 5, 2023, 11:48 PM
 */

#include "Teacher.h"

Teacher::Teacher() {
}

Teacher::Teacher(string sNo, string u, string p):User(u, p) {
    staffNo = sNo;
}

void Teacher::DisplayInfo(){
    cout << "Staff No: " << staffNo << endl; 
    User::DisplayInfo();
}

void Teacher::ViewSummary(vector<QuestionPool*> questionPools){
    int index = 1;
    for(QuestionPool* qp : questionPools){
        cout << index << ". " << qp->Summary();
        index += 1;
    }
}

void Teacher::CreateQuestionPool(vector<QuestionPool*> &questionPools){
    string pName = App::GetString("Please enter a question pool name: ");
    int numOfQ = App::GetInt("Please enter the number of questions: ");
                                
    QuestionPool* qp = new QuestionPool(pName);
    
    for(int i = 1; i <= numOfQ; i++){
        cout << "Creating question " << i << "..." << endl;
        AddQuestion(qp);
    }
    
    questionPools.push_back(qp);
}

void Teacher::RemoveQuestionPool(int i, vector<QuestionPool*> &questionPools){
    if (i >= 0 && i < questionPools.size()) {
        questionPools.erase(questionPools.begin() + i);
    }
    else{
        cout << "Invalid index" << endl;
    }
}

//    to question pool
void Teacher::AddQuestion(QuestionPool* qp){
    string qText = App::GetString("Please enter a question: ");
    vector<Answer*> as;

    for (int i = 1; i <= 4; i++){
        string aText = App::GetString("Please enter an answer: ");
        Answer::AnswerChoice ac = Answer::intToAnswerChoice(i);

        Answer* a = new Answer(ac, aText);
        as.push_back(a);
    }
    int c = App::GetInt("Please enter the index of correct answer(1-4)") - 1;

    Question* q = new Question(qText, as, c);
    
    qp->AddQuestion(q);
}

//    to quiz
void Teacher::AddQuestion(Quiz* qz, int i){
    qz->AddQuestion(i);
}

void Teacher::RemoveQuestion(QuestionPool* qp, int i){
    qp->RemoveQuestion(i);
}

void Teacher::RemoveQuestion(Quiz* qz, int i){
    qz->RemoveQuestion(i);
}

void Teacher::ReviewQuiz(Quiz* qz){
    qz->DisplayReview();
}

void Teacher::ReviewAttempt(Attempt* a){
    a->DisplayReviewSummary();
}

void Teacher::ReviewQuestionPool(QuestionPool* qp, vector<QuestionPool*> questionPools){
    ViewSummary(questionPools);
    qp->DisplayQuestionsSummary();
}

void Teacher::CreateQuiz(vector<QuestionPool*> questionPools, vector<Quiz*> &quizzes){
    string qTitle = App::GetString("Please enter a quiz title: ");
    int numOfQ = App::GetInt("Please enter the number of questions: ");

    ViewSummary(questionPools);
    int qpIndex = App::GetInt("Please select the index of question pool: ") - 1;
    QuestionPool* qp = questionPools[qpIndex];

    int al = App::GetInt("Please enter the quiz attempt limit: ");
    
    Quiz* qz = new Quiz(qTitle, qp, al);
    
    for(int i = 1; i <= numOfQ; i++){
        cout << "Adding question " << i << "..." << endl;
        qp->DisplayQuestionsSummary();
        int qIndex = App::GetInt("Please select the index of question to be added: ") - 1;
        
        qz->AddQuestion(qIndex);
    }
    quizzes.push_back(qz);
}

void Teacher::PublishQuiz(Quiz* qz){
    qz->SetIsPublished(true);
}

void Teacher::RemoveQuiz(int i, vector<Quiz*> &quizzes){
    if (i >= 0 && i < quizzes.size()) {
        quizzes.erase(quizzes.begin() + i);
    }
    else{
        cout << "Invalid index" << endl;
    }
}

Teacher::Teacher(const Teacher& orig) {
}

Teacher::~Teacher() {
}

