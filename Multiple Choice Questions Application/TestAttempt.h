/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/*
 * File:   TestAttempt.h
 * Author: Herman
 *
 * Created on May 11, 2023, 4:09:43 AM
 */

#ifndef TESTATTEMPT_H
#define TESTATTEMPT_H

#include <cppunit/extensions/HelperMacros.h>
#include "Attempt.h"

class TestAttempt : public CPPUNIT_NS::TestFixture {
    CPPUNIT_TEST_SUITE(TestAttempt);

    CPPUNIT_TEST(testGetScore);

    CPPUNIT_TEST_SUITE_END();

public:
    TestAttempt();
    virtual ~TestAttempt();
    void setUp();
    void tearDown();

private:
    void testGetScore();
};

#endif /* TESTATTEMPT_H */

