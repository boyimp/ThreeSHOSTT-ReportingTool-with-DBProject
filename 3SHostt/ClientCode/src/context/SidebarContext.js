import React, { createContext, useContext, useState } from 'react';
import { useMediaQuery } from '@chakra-ui/react';
const SidebarContext = createContext();

const SidebarContextProvider = ({ children }) => {
    const [isLgDevice] = useMediaQuery('(min-width: 62em)');
    const [sidebarOpen, setSidebarOpen] = useState(isLgDevice ? true : false);

    return (
        <SidebarContext.Provider value={{ isLgDevice, sidebarOpen, setSidebarOpen }}>
            {children}
        </SidebarContext.Provider>
    );
};

const useSidebarContext = () => {
    return useContext(SidebarContext);
};

export { SidebarContextProvider, useSidebarContext };
