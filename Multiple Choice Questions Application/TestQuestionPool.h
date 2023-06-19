/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/*
 * File:   TestQuestionPool.h
 * Author: Herman
 *
 * Created on May 11, 2023, 4:11:03 AM
 */

#ifndef TESTQUESTIONPOOL_H
#define TESTQUESTIONPOOL_H

#include <cppunit/extensions/HelperMacros.h>
#include "QuestionPool.h"

class TestQuestionPool : public CPPUNIT_NS::TestFixture {
    CPPUNIT_TEST_SUITE(TestQuestionPool);

    CPPUNIT_TEST(testAddQuestion);
    CPPUNIT_TEST(testRemoveQuestion);
    CPPUNIT_TEST(testSummary);

    CPPUNIT_TEST_SUITE_END();

public:
    TestQuestionPool();
    virtual ~TestQuestionPool();
    void setUp();
    void tearDown();

private:
    void testAddQuestion();
    void testRemoveQuestion();
    void testSummary();
};

#endif /* TESTQUESTIONPOOL_H */

