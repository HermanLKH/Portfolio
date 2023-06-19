/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/* 
 * File:   UserManagement.h
 * Author: Herman
 *
 * Created on May 6, 2023, 3:41 PM
 */

#ifndef USERMANAGEMENT_H
#define USERMANAGEMENT_H
#pragma once
#include <string>
#include <vector>
#include <iostream>

#include "Teacher.h"
#include "Student.h"

using namespace std;

class UserManagement {
public:
    UserManagement();
    
    User* ValidateLogin(App &app);
    void RegisterUser();
    vector<User*> GetStudents();
    
    UserManagement(const UserManagement& orig);
    virtual ~UserManagement();
private:
    vector<User*> students;
    vector<User*> teachers;
    
    void registerStudent();
    void registerTeacher();
};

#endif /* USERMANAGEMENT_H */

