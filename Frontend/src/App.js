import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import {Login} from './components/Login'
import { Registration } from './components/Registration';
import { Cabinet } from './components/Cabinet';
import { CreateQuestion } from './components/CreateQuestion';
import { EditUser } from './components/EditUser';
import {Users} from './components/Users';
import { Question } from './components/Question';
import './custom.css'
import { Questions } from './components/Questions';
import { OwnQuestions } from './components/OwnQuestions';
import { AddTest } from './components/AddTest';
import { EnrollTest } from './components/EnrollTest';
import { Test } from './components/Test';
import { CheckResults } from './components/CheckResults';
import { OwnTests } from './components/OwnTests';
import { PassTest } from './components/PassTest';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route exact path='/login' component={Login} />
        <Route exact path ='/registration' component={Registration}/>
        <Route exact path ='/cabinet' component={Cabinet}/>
        <Route exact path ='/questions' component={Questions}/>
        <Route exact path ='/myQuestions' component={OwnQuestions}/>
        <Route exact path ='/questions/:id' component={Question}/>
        <Route exact path ='/myQuestions/add' component={CreateQuestion}/>
        <Route exact path ='/myTests/add' component={AddTest}/>
        <Route exact path ='/myTests' component={OwnTests}/>
        <Route exact path ='/myTests/check' component={CheckResults}/>
        <Route exact path ='/tests/:id/enroll' component={EnrollTest}/>
        <Route exact path ='/tests/:id/' component={Test}/>
        <Route exact path ='/tests/:id/passtest' component={PassTest}/>
        <Route exact path ='/users/:id' component={EditUser}/>
        <Route exact path ='/users' component={Users}/>
        
      </Layout>
    );
  }
}
