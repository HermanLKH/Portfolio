/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/*
 * File:   TestAttempt.cpp
 * Author: Herman
 *
 * Created on May 11, 2023, 4:09:43 AM
 */

#include "TestAttempt.h"


CPPUNIT_TEST_SUITE_REGISTRATION(TestAttempt);

TestAttempt::TestAttempt() {
}

TestAttempt::~TestAttempt() {
}

void TestAttempt::setUp() {
}

void TestAttempt::tearDown() {
}

void TestAttempt::testGetScore() {
    QuestionPool* qp = new QuestionPool("QP 1");
    
    vector<Answer*> as = {
        new Answer(Answer::A, "Answer 1"),
        new Answer(Answer::B, "Answer 2"),
        new Answer(Answer::C, "Answer 3"),
        new Answer(Answer::D, "Answer 4")
    };
    
    Question* q1 = new Question("Question 1", as, 0);
    Question* q2 = new Question("Question 2", as, 1);
    qp->AddQuestion(q1);
    qp->AddQuestion(q2);
    
    Quiz* qz = new Quiz("Quiz 1", qp, 2);
    qz->AddQuestion(0);
    qz->AddQuestion(1);
    qz->SetIsPublished(true);
    
    for (Question* q : qz->GetQuestions()){
        q->SetAnswerChosen("A");
    }
    
    Attempt* a = new Attempt(qz, 1);
    a->SetScores();
    cout << a->GetScore() << endl;
    CPPUNIT_ASSERT(a->GetScore() == 1);
}
