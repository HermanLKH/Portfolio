/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/*
 * File:   TestQuiz.cpp
 * Author: Herman
 *
 * Created on May 11, 2023, 4:09:10 AM
 */

#include "TestQuiz.h"
#include "Quiz.h"


CPPUNIT_TEST_SUITE_REGISTRATION(TestQuiz);

TestQuiz::TestQuiz() {
}

TestQuiz::~TestQuiz() {
}

void TestQuiz::setUp() {
}

void TestQuiz::tearDown() { 
}

void TestQuiz::testAddQuestion() {
    QuestionPool* qp = new QuestionPool("QP 1");
    
    vector<Answer*> as = {
        new Answer(Answer::A, "Answer 1"),
        new Answer(Answer::B, "Answer 2"),
        new Answer(Answer::C, "Answer 3"),
        new Answer(Answer::D, "Answer 4")
    };
    
    Question* q1 = new Question("Question 1", as, 1);
    Question* q2 = new Question("Question 2", as, 2);
    qp->AddQuestion(q1);
    qp->AddQuestion(q2);
    
    Quiz* qz = new Quiz("Quiz 1", qp, 2);
    CPPUNIT_ASSERT(qz->GetQuestions().size() == 0);
    
    qz->AddQuestion(0);
    qz->AddQuestion(1);
    CPPUNIT_ASSERT(qz->GetQuestions().size() == 2);
}

void TestQuiz::testRemoveQuestion() {
    QuestionPool* qp = new QuestionPool("QP 1");
    
    vector<Answer*> as = {
        new Answer(Answer::A, "Answer 1"),
        new Answer(Answer::B, "Answer 2"),
        new Answer(Answer::C, "Answer 3"),
        new Answer(Answer::D, "Answer 4")
    };
    
    Question* q1 = new Question("Question 1", as, 1);
    Question* q2 = new Question("Question 2", as, 2);
    qp->AddQuestion(q1);
    qp->AddQuestion(q2);
    
    Quiz* qz = new Quiz("Quiz 1", qp, 2);
    qz->AddQuestion(0);
    qz->AddQuestion(1);
    CPPUNIT_ASSERT(qz->GetQuestions().size() == 2);
    
    qz->RemoveQuestion(1);
    qz->RemoveQuestion(0);
    CPPUNIT_ASSERT(qz->GetQuestions().size() == 0);
}

void TestQuiz::testMark() {
    QuestionPool* qp = new QuestionPool("QP 1");
    
    vector<Answer*> as = {
        new Answer(Answer::A, "Answer 1"),
        new Answer(Answer::B, "Answer 2"),
        new Answer(Answer::C, "Answer 3"),
        new Answer(Answer::D, "Answer 4")
    };
    
    Question* q1 = new Question("Question 1", as, 1);
    Question* q2 = new Question("Question 2", as, 2);
    qp->AddQuestion(q1);
    qp->AddQuestion(q2);
    
    Quiz* qz = new Quiz("Quiz 1", qp, 2);
    qz->AddQuestion(0);
    qz->AddQuestion(1);
    qz->SetIsPublished(true);
    
    for (Question* q : qz->GetQuestions()){
        q->SetAnswerChosen("B");
    }
    
    vector<int> expectedResult = {1, 0};
    
    CPPUNIT_ASSERT(qz->Mark() == expectedResult);
}
