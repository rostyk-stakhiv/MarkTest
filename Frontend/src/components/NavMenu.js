import React, { Component } from 'react';
import { Container, Navbar, NavbarBrand, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  Logout() {
    localStorage.clear();
    document.location.href = "/login";
  }
  render() {
    return (
      <header>
        <Navbar className="navbar" light>
          <Container className="menu">
            <NavbarBrand  tag={Link} to="/"><h1 className='MainLink'>MarkTest</h1></NavbarBrand>

            


            {localStorage["isAutorized"] ?
              <div className="IsLogin">
                {localStorage["isTeacher"]==="true"&&<NavLink tag={Link} className="selection" to="/questions"> Питання</NavLink>}
                <NavLink tag={Link} className="selection" to="/cabinet">Кабінет</NavLink>
                <button onClick={this.Logout}>Вийти</button>
              </div> :
              <NavLink tag={Link} className="selection" to="/login">Увійти</NavLink>}
          </Container>
        </Navbar>
      </header>
    );
  }
}
