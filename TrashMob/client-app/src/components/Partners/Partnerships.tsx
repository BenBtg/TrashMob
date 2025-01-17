import { FC } from 'react'
import { Col, Container, Image, Row } from 'react-bootstrap';
import globes from '../assets/gettingStarted/globes.png';
import heroImg from '../assets/partnerships/whatIsPartnerships.png';
import Safetykits from '../assets/partnerships/Safetykits.svg';
import Supplies from '../assets/partnerships/Supplies.svg';
import TrashDisposal from '../assets/partnerships/TrashDisposal.svg';
import Dollarsign from '../assets/partnerships/dollarsign.svg';
import Garbage from '../assets/partnerships/garbage.png';
import { Link } from 'react-router-dom';
import React from 'react';

export const Partnerships: FC<any> = () => {

    React.useEffect(() => {
        window.scrollTo(0, 0);
    })

    return (
        <>
            <Container fluid className='bg-grass mb-5'>
                <Row className="text-center pt-0">
                    <Col md={7} className="d-flex flex-column justify-content-center pr-5">
                        <h1 className="font-weight-bold">Partnerships</h1>
                        <p className="font-weight-bold">Connecting you with volunteers.</p>
                    </Col>
                    <Col md={5}>
                        <Image src={globes} alt="globes" className="h-100 mt-0" />
                    </Col>
                </Row>
            </Container>
            <Container className='py-5'>
                <Row>
                    <Col sm={7}>
                        <h1 className='fw-600'>What are partnerships?</h1>
                        <h4>Partnering with local cities and businesses can connect TrashMob event volunteers with the supplies and services they need.</h4>
                        <p>Partners can include cities, local businesses, and branches/locations of larger companies. Services can include trash hauling and disposal locations, and supplies can include buckets, grabber tools, and safety equipment.
                            Looking for supplies and services for your next event? Invite a partnership from your city! Have supplies and services to offer? Become a partner!</p>
                    </Col>
                    <Col sm={5}>
                        <Image src={heroImg} alt="garbage being loaded into garbage truck" className="mt-0 h-100" />
                    </Col>
                </Row>
            </Container>
            <div className='w-100 bg-white'>
                <Container className='py-5 px-0'>
                    <Row className="max-width-container mx-auto align-items-center">
                        <Col sm={6}>
                            <div className='d-flex flex-column align-items-start'>
                                <p className="font-size-h4">No partner for your event? Invite local government or business to join TrashMob.eco as a partner!</p>
                                <Link className="btn btn-primary banner-button" to="/inviteapartner">Invite a partner</Link>
                            </div>
                        </Col>
                        <Col sm={6}>
                            <div className='d-flex flex-column align-items-start'>
                                <p className="font-size-h4">
                                    Have supplies and services to offer? Submit an application to become a TrashMob.eco partner!
                                </p>
                                <Link className="btn btn-primary banner-button" to="/becomeapartner">Become a partner</Link>
                            </div>
                        </Col>
                    </Row>
                </Container>
            </div>
            <Container fluid className='text-center py-5 px-0'>
                <h2 className='font-weight-bold mb-3'>Partnerships support the volunteers</h2>
                <span>Services and supplies offered can include:</span>
                <Row className="justify-content-center">
                    <Row className='w-50 mt-5 mx-auto justify-content-around    '>
                        <Col className="d-flex flex-column align-items-center">
                            <Image src={Safetykits} alt="Safety kits" className="graphic-large mt-0" />
                            <span className="font-weight-bold mt-2">Safety gear and roadside signs</span>
                        </Col>
                        <Col className="d-flex flex-column align-items-center">
                            <Image src={Supplies} alt="Supplies" className="graphic-large mt-0" />
                            <span className="font-weight-bold mt-2">Pickup supplies such as garbage bags</span>
                        </Col>
                        <Col className="d-flex flex-column align-items-center">
                            <Image src={TrashDisposal} alt="Trash Disposal & Hauling" className="graphic-large mt-0" />
                            <span className="font-weight-bold mt-2">Use of existing dumpsters and hauling of trash to disposal site</span>
                        </Col>
                        <Col className="d-flex flex-column align-items-center">
                            <Image src={Dollarsign} alt="Dollar sign" className="graphic-large mt-0" />
                            <span className="font-weight-bold mt-2"><a href="https://www.trashmob.eco/donate">Donations</a> to TrashMob.eco fund development of our platform and programs</span>
                        </Col>
                    </Row>

                </Row>
            </Container>

            <Container fluid className='bg-white py-5'>
                <Row className="container mx-auto px-0">
                    <Col sm={7}>
                        <h1 className='fw-600'>Making the most out of partnerships</h1>
                        <p className='para'>A successful clean up event depends upon a team of volunteers and the support of partners: community businesses, organizations and governments.  Volunteer organizers set an event location, rally member support and utilize partnership provisions.</p>
                        <p className='para'>TrashMob administrators confirm, approve and connect partners with event organizers. Partners and their form of support are indicated on event registration pages. Then local teamwork commences!  Event organizers and partners coordinate access of supplies, services and instructions. Partners are selected upon availability and proximity to the event. Note: Supplied services from a given partner may vary by location/branch. </p>
                    </Col>
                    <Col sm={5}>
                        <Image src={Garbage} alt="garbage bags being picked up" className="mh-100 mt-0" />
                    </Col>
                </Row>
            </Container>
        </>
    );
}
