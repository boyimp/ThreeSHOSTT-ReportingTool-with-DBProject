/* eslint-disable react-hooks/rules-of-hooks */
import {
    Button,
    Checkbox,
    FormControl,
    FormLabel,
    Box,
    Image,
    Input,
    Text,
    VStack,
    Flex,
    Alert,
    AlertDescription,
    AlertIcon,
    Stack,
    LightMode
} from '@chakra-ui/react';
import React, { useEffect, useLayoutEffect, useState } from 'react';
import User from '../../models/user';
import { useHistory } from 'react-router-dom';
import { isSessionActive } from './login.action';
import login from './login.action';

const Login = () => {
    const history = useHistory();
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [errorMsg, setErrorMsg] = useState();
    const [loading, setLoading] = useState(false);

    const handleLogin = async () => {
        let user = new User();
        user.Username = username;
        user.Password = password;

        setLoading(true);
        let response = await login(user);
        setLoading(false);

        if (response === true) {
            history.replace('/dashboard');
        }
        if (username && password) {
            if (response.response.status === 404) {
                setErrorMsg('Wrong Credential');
            }
            if (response.response.status === 500) {
                setErrorMsg('Database Not Found');
            }
        } else if (!username) {
            setErrorMsg('Enter Your  Username');
        } else if (!password) {
            setErrorMsg('Enter Your Password');
        } else {
            setErrorMsg(null);
        }
    };

    useEffect(() => {
        async function fetchSession() {
            var session = await isSessionActive();
            if (session) {
                console.log('Sesson : ' + session);
                history.replace('/dashboard');
            }
        }
        fetchSession();
    }, []);

    return (
        <>
            <Flex
                w="100vw"
                h="100vh"
                justify="center"
                align="center"
                className="loginContainer"
                bgImage="./Images/loginBg6.jpg">
                <Flex bg="#00000070" w="100%" h="100%" justify="center" align="center">
                    <Stack
                        w={{ base: '90%', sm: '80%', md: '60%', lg: '50%' }}
                        minH="500px"
                        bg="white"
                        boxShadow="lg"
                        borderRadius="20px"
                        position="relative"
                        align="center"
                        justify="center">
                        <Flex direction={{ base: 'column', lg: 'row' }} align="center">
                            <Flex
                                w="50%"
                                h="100%"
                                align="center"
                                justify="center"
                                d={{ base: 'none', lg: 'flex' }}>
                                <Image src="./Images/loginImg2.png" w="100%" alt="login" />
                            </Flex>

                            <VStack w={{ base: '100%', lg: '50%' }} px="10" mb="10" spacing="5">
                                <Flex direction="column" align="center">
                                    <Flex justify="center" align="center">
                                        <Image w="70px" src="./Images/3SLogo1.png" />
                                        <Box h="20px" w="1px" bg="gray.800" mx="15px"></Box>
                                        <Image w="80px" src="./Images/3S_Hostt.png" />
                                    </Flex>

                                    <Text
                                        fontWeight="bold"
                                        my="10px"
                                        color="gray.800"
                                        fontSize="13px">
                                        Taking Technology Forward
                                    </Text>
                                </Flex>

                                {errorMsg && (
                                    <LightMode>
                                        <Alert status="error">
                                            <AlertIcon />
                                            <AlertDescription>
                                                <Text color="black">{errorMsg}</Text>
                                            </AlertDescription>
                                        </Alert>
                                    </LightMode>
                                )}

                                <FormControl>
                                    <FormLabel color="gray.800">Username</FormLabel>
                                    <Input
                                        placeholder="Enter your username"
                                        value={username}
                                        onChange={(event) => setUsername(event.target.value)}
                                        color="gray.800"
                                        _placeholder={{ color: 'gray.400' }}
                                        borderColor="gray.200"
                                    />
                                </FormControl>

                                <FormControl>
                                    <FormLabel color="gray.800">Password</FormLabel>
                                    <Input
                                        type="password"
                                        placeholder="Enter your password"
                                        value={password}
                                        onChange={(event) => setPassword(event.target.value)}
                                        color="gray.800"
                                        _placeholder={{ color: 'gray.400' }}
                                        borderColor="gray.200"
                                    />
                                </FormControl>

                                {/* <FormControl>
                                    <Checkbox color="gray.800" borderColor="gray.200">
                                        Remember me
                                    </Checkbox>
                                </FormControl> */}

                                <LightMode>
                                    <Button
                                        width="full"
                                        size="lg"
                                        colorScheme="orange"
                                        onClick={handleLogin}
                                        disabled={loading}>
                                        {loading ? 'Signing in..' : 'Sign In'}
                                    </Button>
                                </LightMode>
                            </VStack>
                        </Flex>

                        <Flex align="center" justify="center">
                            <Text color="gray.600" fontWeight="500" letterSpacing="1px">
                                Powered By 3S
                            </Text>
                        </Flex>
                    </Stack>
                </Flex>
            </Flex>
        </>
    );
};

export default Login;
