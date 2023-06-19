/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   main.cpp
 * Author: Herman
 *
 * Created on May 5, 2023, 9:12 PM
 */

#include <cstdlib>
#include <iostream>

#include "App.h"
#include "User.h"
#include "UserManagement.h"
#include "Attempt.h"

using namespace std;

/*
 * 
 */
int main(int argc, char** argv) {
    string opt;
    UserManagement* um = new UserManagement();
    
    vector<QuestionPool*> questionPools;
    vector<Quiz*> quizzes;
    
    App* quizApp = new App();
    User* user = nullptr;

    while(quizApp->GetIsActive()){
        
        opt = App::GetString("1. Register\n2. Login\n3. Exit\n");
        
        if(opt == "1"){
            um->RegisterUser();
        }
        else if(opt == "2"){
            user = um->ValidateLogin(*quizApp);
        }
        else if(opt == "3"){
            quizApp->SetIsActive(false);
        }
        else{
            cout << "Invalid input" << endl;
        }
        
        while(user != nullptr){
            Teacher* t = dynamic_cast<Teacher*>(user);
            Student* s = dynamic_cast<Student*>(user);
            
            if(t != nullptr) {
                quizApp->SetUserMode(App::Teacher);
                quizApp->DisplayMenu();
                
                switch(quizApp->GetOpt()){
                    //    Question Pool
                    case 1:{
                        switch(quizApp->GetSubOpt()){
                            //    Create question pool
                            case 1:{
                                cout << "Creating new question pool..." << endl;
                                t->CreateQuestionPool(questionPools);
                                break;
                            }
                            //    Delete question pool
                            case 2:{
                                t->ViewSummary(questionPools);
                                int qpIndex = App::GetInt("Please select the index of question pool to be deleted: ") - 1;
                                
                                t->RemoveQuestionPool(qpIndex, questionPools);
                                break;
                            }
                            //    Add question to question pool
                            case 3:{
                                t->ViewSummary(questionPools);
                                int qpIndex = App::GetInt("Please select the index of question pool: ") - 1;
                                QuestionPool* qp = questionPools[qpIndex];
                                
                                t->AddQuestion(qp);
                                break;
                            }
                            //    Delete question from question pool
                            case 4:{
                                t->ViewSummary(questionPools);
                                int qpIndex = App::GetInt("Please select the index of question pool: ") - 1;
                                QuestionPool* qp = questionPools[qpIndex];
                                
                                qp->DisplayQuestionsSummary();
                                int qIndex = App::GetInt("Please select the index of question to be deleted: ") - 1;
                                
                                t->RemoveQuestion(qp, qIndex);
                                break;
                            }
                            //    View question pool
                            case 5:{
                                t->ViewSummary(questionPools);
                                int qpIndex = App::GetInt("Please select the index of question pool: ") - 1;
                                QuestionPool* qp = questionPools[qpIndex];
                                t->ReviewQuestionPool(qp, questionPools);
                                
                                break;
                            }
                            //    Exit
                            case 6:{
                                break;
                            }
                        }
                        break;
                    }
                    //    Quiz
                    case 2:{                
                        switch(quizApp->GetSubOpt()){
                            //    Create quiz
                            case 1:{
                                cout << "Creating new quiz..." << endl;
                                
                                t->CreateQuiz(questionPools, quizzes);
                                break;
                            }
                            //    Remove quiz
                            case 2:{
                                if(t->ViewQuizzesSummary(false, quizzes)){
                                    int qzIndex = App::GetInt("Please select the index of quiz: ") - 1;
                                    t->RemoveQuiz(qzIndex, quizzes);
                                }
                                break;
                            }
                            //    Add question to quiz
                            case 3:{
                                if(t->ViewQuizzesSummary(false, quizzes)){
                                    int qzIndex = App::GetInt("Please select the index of quiz: ") - 1;
                                    Quiz* qz = quizzes[qzIndex];

                                    qz->GetQuestionPool()->DisplayQuestionsSummary();
                                    int qIndex = App::GetInt("Please select the index of question to be added: ") - 1;

                                    t->AddQuestion(qz, qIndex);
                                }
                                break;
                            }
                            //    Delete question from quiz
                            case 4:{
                                if(t->ViewQuizzesSummary(false, quizzes)){
                                    int qzIndex = App::GetInt("Please select the index of quiz: ") - 1;
                                    Quiz* qz = quizzes[qzIndex];

                                    qz->DisplayQuestionsSummary();
                                    int qIndex = App::GetInt("Please select the index of question to be removed: ") - 1;

                                    t->RemoveQuestion(qz, qIndex);
                                }
                                break;
                            }
                            //    Publish quiz
                            case 5:{
                                if(t->ViewQuizzesSummary(false, quizzes)){
                                    int qzIndex = App::GetInt("Please select the index of quiz: ") - 1;
                                    Quiz* qz = quizzes[qzIndex];

                                    t->PublishQuiz(qz);
                                }
                                break;
                            }
                            //    Review quiz
                            case 6:{
                                if(t->ViewQuizzesSummary(false, quizzes)){
                                    int qzIndex = App::GetInt("Please select the index of quiz: ") - 1;
                                    Quiz* qz = quizzes[qzIndex];

                                    t->ReviewQuiz(qz);
                                }
                                break;
                            }
                            //    View students attempts
                            case 7:{
                                int index = 1;
                                
                                for(User* u : um->GetStudents()){
                                    if(Student* s = dynamic_cast<Student*>(u)){
                                        for(Attempt* a : s->GetAttempts()){
                                            cout << index << ". " << s->GetStudId() << "\t";
                                            t->ReviewAttempt(a);
                                            index += 1;
                                        }
                                    }
                                }
                                if(index == 1){
                                    cout << "No attempt yet" << endl;
                                }
                                break;
                            }
                            case 8:{
                                break;
                            }
                        }
                        break;
                    }
                    //    View profile info
                    case 3:{
                        t->DisplayInfo();
                        break;
                    }
                    //    Logout
                    case 4:{
                        user = nullptr;
                        break;
                    }
                }
            }
            else if(s != nullptr){
                quizApp->SetUserMode(App::Student);
                quizApp->DisplayMenu();
                
                switch(quizApp->GetOpt()){
                    case 1:{
                        if(s->ViewQuizzesSummary(true, quizzes)){
                            int qzIndex = App::GetInt("Please select the index of quiz") - 1;
                            Quiz* qz = quizzes[qzIndex];

                            s->AttemptQuiz(qz);
                        }
                        break;
                    }
                    case 2:{
                        if(s->ViewQuizzesSummary(true, quizzes)){
                            int qzIndex = App::GetInt("Please select the index of quiz") - 1;
                            Quiz* qz = quizzes[qzIndex];

                            vector<Attempt*> attemptsBQ = s->GetAttemptsByQuiz(qz);
                            int aIndex = App::GetInt("Please select the index of attempt") - 1;
                            Attempt* a = attemptsBQ[aIndex];

                            s->ReviewAttempt(a);
                        }
                        break;
                    }
                    case 3:{
                        s->DisplayInfo();
                        break;
                    }
                    case 4:{
                        user = nullptr;
                        break;
                    }
                }
            }
        }
        
    }
    
    delete um;
    delete user;
    delete quizApp;
    
    return 0;
}

